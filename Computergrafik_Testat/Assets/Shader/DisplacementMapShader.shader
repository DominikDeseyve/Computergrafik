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
		_WaterMap ("Water Map", 2D) = "normal" {}

		//LambertShader
		// Definition der Hauptfarbe.
		_Color ("Base Color", Color) = (1,1,1,1)	
		// Reflektanz des Ambienten Licht
		_Ka("Ambient Reflectance", Range(0, 1)) = 0.5
		// Reflektanz des Diffusen Licht
		_Kd("Diffuse Reflectance", Range(0, 1)) = 0.5
		
        _Scale ("Terrain Scale", float) = 0.5			//Höhe der Berge
		_BasisHeight("Basis Höhe", float) = 0.5
		_Modus ("GameObject", Range(0, 1)) = 0
		_LiquidScale ("Liquid Scale", Range(0, 1)) = 0	//gibt Höhe des Wasserspiegels an

	}

	// A Shader can contain one or more SubShaders, which are primarily used to implement shaders for different GPU capabilities
	SubShader
	{
		// Subshaders use tags to tell how and when
		// they expect to be rendered to the rendering engine.
		// https://docs.unity3d.com/Manual/SL-SubShaderTags.html
		Tags { "RenderType"="Opaque" }

		// Each SubShader is composed of a number of passes, and each Pass represents an execution of the vertex and fragment
		// code for the same object rendered with the material of the shader
		Pass
		{
			// CGPROGRAM ... ENDCG
			// These keywords surround portions of HLSL code within the vertex and fragment shaders
			CGPROGRAM

			// Definition shaders used and their function names
			#pragma vertex vert
			#pragma fragment frag

			// Builtin Includes
			// https://docs.unity3d.com/Manual/SL-BuiltinIncludes.html
			#include "UnityCG.cginc"

			sampler2D _HeightMap;
			float4 _HeightMap_ST;
			float _Scale;

			sampler2D _ColorMap;

			sampler2D _MoistureMap;

			float _LiquidScale;
			float _Modus;
			float _BasisHeight;

            // Declare our new parameter here so it's visible to the CG shader
            float4 _ScrollSpeeds;

			// struct to pass Data from Vertex Sahder to Fragment Shader
			struct v2f
			{
				// SV_POSITION: Shader semantic for position in Clip Space: https://docs.unity3d.com/Manual/SL-ShaderSemantics.html?_ga=2.64760810.432960686.1524081652-394573263.1524081652
				float4 vertex : SV_POSITION;
				float4 col : COLOR;
			};

			float _MaxDepth;

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
				fixed4 moisture = tex2Dlod(_MoistureMap, float4(v.texcoord.xy, 0, 0));

				float moistureLength = sqrt(float(moisture.x)*float(moisture.x)+float(moisture.y)*float(moisture.y)+float(moisture.z)*float(moisture.z));
				if (height <= _LiquidScale) 
				{
					height = _LiquidScale;
					o.col = tex2Dlod(_ColorMap, float4(0.05, 0.05, 0.0, 0.0));   //x(moisture), y(height), 0, 0
				} else 
				{
					o.col = tex2Dlod(_ColorMap, float4(moistureLength, height, 0.0, 0.0));   //x(moisture), y(height), 0, 0
				}

				switch (_Modus){
					case 0:	vertexPos.xyz += normalize(vertexPos.xyz)*(height*_Scale+_BasisHeight);
					break;
					case 1: vertexPos.xyz += v.normal*(height*_Scale);
					break;
				}	
				// Convert Vertex Data from Object to Clip Space
				o.vertex = UnityObjectToClipPos(vertexPos);
				
				return o;
			}

			// FRAGMENT / PIXEL SHADER
			// SV_Target: Shader semantic render target (SV_Target = SV_Target0): https://docs.unity3d.com/Manual/SL-ShaderSemantics.html?_ga=2.64760810.432960686.1524081652-394573263.1524081652
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = i.col;

				return col;
			}
			ENDCG
		}
	}
}
