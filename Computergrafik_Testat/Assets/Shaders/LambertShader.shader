﻿Shader "CG_Lecture/LambertShader"
{
	// Tutorial - Vertex und Fragment Shader examples: https://docs.unity3d.com/Manual/SL-VertexFragmentShaderExamples.html 

	Properties
	{
		// Definition der Hauptfarbe.
		_Color ("Base Color", Color) = (1,1,1,1)
		
		// Reflektanz des Ambienten Licht
		_Ka("Ambient Reflectance", Range(0, 1)) = 0.5

		// Reflektanz des Diffusen Licht
		_Kd("Diffuse Reflectance", Range(0, 1)) = 0.5

	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
        LOD 100

		Pass
		{
			// indicate that our pass is the "base" pass in forward
            // rendering pipeline. It gets ambient and main directional
            // light data set up; light direction in _WorldSpaceLightPos0
            // and color in _LightColor0
            Tags {"LightMode"="ForwardBase"}

			CGPROGRAM

			// Definition über die Shader die verwendet werden, und wie sie heißen
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "Lighting.cginc" // für Lighting

			// Struct zum Austausch der Daten zwischen Vertex und Fragment Shader
			struct v2f
			{
				// Weitergabe der konvertierten Vertex Positionen in Homogenen Koordinaten
				float4 vertex : SV_POSITION;

				// Oberflächen Normalen
				half3 worldNormal : TEXCOORD0;
			};

			// Zu verwendende Farbe
            fixed4 _Color;
			float _Ka, _Kd;
			
			// VERTEX SHADER
			// 'appdata_base' ist ein standard struct das genutzt werden kann um den Vertex Shader mit Daten zu füttern
			// https://docs.unity3d.com/Manual/SL-VertexProgramInputs.html
			// http://wiki.unity3d.com/index.php?title=Shader_Code
			v2f vert (appdata_base vertexIn)
			{
				v2f vertexOut;

				// Transformation der Vertices aus Objekt-Koordinaten in Clip-Koordinaten
				// TODO
				vertexOut.vertex = UnityObjectToClipPos(vertexIn.vertex);

				// Tranformation der Normalen-Vektoren in Welt-Koordinaten
                // TODO		
				vertexOut.worldNormal = UnityObjectToWorldNormal(vertexIn.normal);
				
				return vertexOut;
			}

			// FRAGMENT / PIXEL SHADER
			// Als input erhält er die interpolierten Output-Daten des Vertex-Shaders.
			// Dieser Typ des Fragment Shaders erfordert eigentlich kein Input, da er für jedes Fragment einfach nur eine Farbe zurück gibt.
			// Output ist 4d vector der die Farbe des Fragments/Pixels angibt.
			// SV_Target gibt an, dass es sich shcon um die "Target" Farbe handelt. Alternativ könnte auch das Struct fragmentOut
			// als Rückgabe des Fragment Shaders verwendet werden.
			fixed4 frag (v2f fragIn) : SV_Target
			{
				// Nutze die definierte Farbe.
                fixed4 color = _Color;

				// Ambiente Licht Farbe
				// das gesamte ambiente Licht der Szene wird durch die Funktion ShadeSH9 (Teil von UnityCG.cginc) ausgewertet
				// Dazu werden die homogenen Oberflächen Normalen in Welt-Koordinaten verwendet.
				float4 ambLight = float4(ShadeSH9(half4(fragIn.worldNormal,1)),1);

				// Standard Diffuse (Lambert) Shading
				// Gewichtung durch Skalarprodukt (Dot-Produkt) zwischen Normalen-Vektor
				// Richtung der Beleuchtungsquelle

				// WICHTIG: Bei Direktionalem Licht gibt _WorldSpaceLightPos0 die Richtung der Lichtquelle an. 
				// Bei Anderen Lichtquellen gibt es die Homogenen Koordinaten der Lichtquelle in Welt-Koordinaten an.
				// https://docs.unity3d.com/Manual/SL-UnityShaderVariables.html
                half nl = max(0, dot(fragIn.worldNormal, _WorldSpaceLightPos0.xyz));
                
				// Diffuser Anteil multipliziert mit der Lichtfarbe
                float4 diffLight = nl * _LightColor0;
				
				// Multiplikation der Grundfarbe mit dem Ambienten- und dem Diffusions-Anteil
				// Der Diffuse und Ambiente Anteil wird jeweils mit der entsprechenden Reflektanz der Oberfläche (_Ka, _Kd) gewichtet.
				// TODO
				color *= (_Ka * ambLight + _Kd * diffLight);

                return color;
			}
			ENDCG
		}
	}
}
