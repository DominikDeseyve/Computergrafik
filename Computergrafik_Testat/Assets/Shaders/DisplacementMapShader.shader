Shader "CG_Lecture/DisplacementMapShader"
{
	// Tutorial - Vertex und Fragment Shader examples: https://docs.unity3d.com/Manual/SL-VertexFragmentShaderExamples.html
	// DOC: How to write vertex and fragment shaders: https://docs.unity3d.com/Manual/SL-ShaderPrograms.html

	// Property Definition --> Visible in IDE
	Properties
	{
        _HeightMap ("Height Map", 2D) = "bump" {}
		_MoistureMap ("Moisture Map", 2D) = "normal" {}
		_ColorMap ("Color Map", 2D) = "normal" {}
		_WaterMap1 ("Water Map 1", 2D) = "normal" {}
		_WaterMap2 ("Water Map 2", 2D) = "normal" {}

		//LambertShader
		// Definition der Hauptfarbe.
		//_Color ("Base Color", Color) = (1,1,1,1)	
		// Reflektanz des Ambienten Licht
		_Ka("Ambient Reflectance", Range(0, 1)) = 1
		// Reflektanz des Diffusen Licht
		_Kd("Diffuse Reflectance", Range(0, 1)) = 1
		// Spekulare Reflektanz
		_Ks("Specular Reflectance", Range(0, 1)) = 1
		// Shininess
		_Shininess("Shininess", Range(0.1, 1000)) = 100
		
        _Scale ("Terrain Scale", Range(0, 1000)) = 0.4		//Höhe der Berge
		_BasisHeight("Basis Height", Range(0, 1)) = 0.5
		
		[Toggle]
		_Modus ("useNormals", Float) = 0

		_LiquidScale ("Liquid Scale", Range(0, 1)) = 0.5	//gibt Höhe des Wasserspiegels an
		_WaterSpeed ("WaterSpeed", Range(0,1)) = 0.3
	}

	// A Shader can contain one or more SubShaders, which are primarily used to implement shaders for different GPU capabilities
	SubShader
	{
		// Subshaders use tags to tell how and when
		// they expect to be rendered to the rendering engine.
		// https://docs.unity3d.com/Manual/SL-SubShaderTags.html
		Tags { "RenderType"="Opaque" }
		LOD 100

		// Each SubShader is composed of a number of passes, and each Pass represents an execution of the vertex and fragment
		// code for the same object rendered with the material of the shader
		Pass
		{
			// CGPROGRAM ... ENDCG
			// These keywords surround portions of HLSL code within the vertex and fragment shaders
			Tags {"LightMode"="ForwardBase"}		
			CGPROGRAM

			// Definition shaders used and their function names
			#pragma vertex vert
			#pragma fragment frag

			// Builtin Includes
			// https://docs.unity3d.com/Manual/SL-BuiltinIncludes.html
			#include "UnityCG.cginc"
			#include "Lighting.cginc" // für Lighting


			// struct to pass Data from Vertex Sahder to Fragment Shader
			struct v2f
			{
				// SV_POSITION: Shader semantic for position in Clip Space: https://docs.unity3d.com/Manual/SL-ShaderSemantics.html?_ga=2.64760810.432960686.1524081652-394573263.1524081652
				float4 vertex : SV_POSITION;
				float4 col : COLOR;

				// Weitergabe der Textur Koordinaten
				float2 uv : TEXCOORD0;

				// Oberflächen Normalen
				half3 worldNormal : TEXCOORD1;

				// Blickrichtung in Welt Koordinaten
				half3 worldViewDir : TEXCOORD2;

				// these three vectors will hold a 3x3 rotation matrix
				// that transforms from tangent to world space
				half3 tspace0 : TEXCOORD3;
				half3 tspace1 : TEXCOORD4;
				half3 tspace2 : TEXCOORD5; 

				bool isLand : TEXCOORD6;	//gibt an ob es sich um Land oder Wasser handelt (LAND == TRUE; WASSER == FALSE)
			};

			//float _MaxDepth;

			sampler2D _HeightMap;
			float4 _HeightMap_ST;
			float _Scale;

			sampler2D _ColorMap;

			sampler2D _MoistureMap;

			sampler2D _WaterMap1, _WaterMap2;

			float _LiquidScale;
			float _WaterSpeed;
			float _Modus;
			float _BasisHeight;

			float _Ka, _Kd, _Ks;
			float _Shininess;
			float4 _Color;

            // Declare our new parameter here so it's visible to the CG shader
            float4 _ScrollSpeeds;

			// VERTEX SHADER
			// https://docs.unity3d.com/Manual/SL-VertexProgramInputs.html
			// http://wiki.unity3d.com/index.php?title=Shader_Code
			v2f vert (appdata_full v)
			{
				v2f o;

				// get vertex Data
				float4 vertexPos = v.vertex;

				// Access texture and extract color value
				float height = tex2Dlod(_HeightMap, float4(v.texcoord.xy, 0, 0)).x;
				float moisture = tex2Dlod(_MoistureMap, float4(v.texcoord.xy, 0, 0)).x;


				if (height <= _LiquidScale) 
				{
					height = _LiquidScale;
					o.isLand = false;
					o.col = tex2Dlod(_ColorMap, float4(0.05, 0.05, 0.0, 0.0));   //x(moisture), y(height), 0, 0
				} else 
				{
					o.isLand = true;
					//o.col = tex2Dlod(_ColorMap, float4(moisture, height - _LiquidScale + 0.1f, 0.0, 0.0));
					o.col = tex2Dlod(_ColorMap, float4(moisture, height, 0.0, 0.0));   //x(moisture), y(height), 0, 0					
				}

				switch (_Modus){
					case 0:	vertexPos.xyz += normalize(vertexPos.xyz) * (height * _Scale / 1000 + _BasisHeight);
					break;
					case 1: vertexPos.xyz += v.normal * (height * _Scale);
					break;
				}	
				
				// Convert Vertex Data from Object to Clip Space
				o.vertex = UnityObjectToClipPos(vertexPos);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);

				// Berechnung der Blickrichtung in Welt Koordinaten
				o.worldViewDir = normalize(WorldSpaceViewDir(vertexPos));

				half3 wNormal = UnityObjectToWorldNormal(v.normal);
				half3 wTangent = UnityObjectToWorldDir(v.tangent);

				// compute bitangent from cross product of normal and tangent
				// bitanget vector is needed to convert the normal from the normal map into world space
				// see: http://www.opengl-tutorial.org/intermediate-tutorials/tutorial-13-normal-mapping/
				half3 wBitangent = cross(wNormal, wTangent);

				// output the tangent space matrix
				o.tspace0 = half3(wTangent.x, wBitangent.x, wNormal.x);
				o.tspace1 = half3(wTangent.y, wBitangent.y, wNormal.y);
				o.tspace2 = half3(wTangent.z, wBitangent.z, wNormal.z); 			

				// output normal vector
				//o.worldNormal = wNormal;

				// output texture coordinates
				o.uv = v.texcoord;
				return o;
			}

			// FRAGMENT / PIXEL SHADER
			// SV_Target: Shader semantic render target (SV_Target = SV_Target0): https://docs.unity3d.com/Manual/SL-ShaderSemantics.html?_ga=2.64760810.432960686.1524081652-394573263.1524081652
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 color = i.col;
				
				// transform normal from tangent to world space
                half3 normal;
				
				// Für Wellenanimation
				if (i.isLand) {
					normal = i.worldNormal;
				} else {
					half3 tnormal = normalize(UnpackNormal(tex2D(_WaterMap1, i.uv - _WaterSpeed*_Time.xx)) + UnpackNormal(tex2D(_WaterMap2, i.uv + _WaterSpeed*0.5*_Time.xx)));
                	normal.x = dot(i.tspace0, tnormal);
                	normal.y = dot(i.tspace1, tnormal);
                	normal.z = dot(i.tspace2, tnormal);
				}

				// Ambiente Licht Farbe
				// das gesamte ambiente Licht der Szene wird durch die Funktion ShadeSH9 (Teil von UnityCG.cginc) ausgewertet
				// Dazu werden die homogenen Oberflächen Normalen in Welt-Koordinaten verwendet.
				float4 amb = float4(ShadeSH9(half4(normal,1)),1);

				// Standard Diffuse (Lambert) Shading
				// Gewichtung durch Skalarprodukt (Dot-Produkt) zwischen Normalen-Vektor
				// Richtung der Beleuchtungsquelle

				// WICHTIG: Bei Direktionalem Licht gibt _WorldSpaceLightPos0 die Richtung der Lichtquelle an. 
				// Bei Anderen Lichtquellen gibt es die Homogenen Koordinaten der Lichtquelle in Welt-Koordinaten an.
				// https://docs.unity3d.com/Manual/SL-UnityShaderVariables.html
                half nl = max(0, dot(normal, _WorldSpaceLightPos0.xyz));
                
				// Diffuser Anteil multipliziert mit der Lichtfarbe
                float4 diff = nl * _LightColor0;


				float3 worldSpaceReflection = reflect(normalize(-_WorldSpaceLightPos0.xyz), normal);
				half re = pow(max(dot(worldSpaceReflection, i.worldViewDir), 0), _Shininess);

				float4 spec = re * _LightColor0;			

				if (i.isLand) {	
					color *= _Ka*amb + _Kd* diff;
					
				} else {
					color *= _Ka*amb + _Kd* diff;
					color += _Ks* spec;
				}	

				return saturate(color);
			}
			ENDCG
		}
	}
}
