Shader "Custom/ColorChangeTest"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		_ColorPaletteOriginal("Colour Palette Original", 2D) = "white"{}
		_ColorPaletteMorph("Colour Palette Morph", 2D) = "white"{}
		_floatTest("Float test", Float) = 0 
		_textureWidth("Texture Width", int) = 64
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma glsl
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile DUMMY PIXELSNAP_ON
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
			};
			
			fixed4 _Color;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _ColorPaletteOriginal;
			sampler2D _ColorPaletteMorph;
			Float _floatTest;
			int _textureWidth;

			fixed4 frag(v2f IN) : COLOR
			{
				//float4 pCoord = tex2Dlod(_MainTex, float4(IN.texcoord.xy, 0.0, 0.0));
				//float palSize = 1.0/_PallitCount;
				//float palShift = IN.color.r*255.0;
				//float4 c = tex2Dlod(_PallitTex, float4(pCoord.r*palSize+palSize*(palShift-1), 0.5, 0.0, 0.0));
				//return c*_Color;
				float4 currentColor= tex2D(_MainTex, IN.texcoord.xy);
				float pos = 0;
				for (float x =0; x <_textureWidth; x +=1){
					float4 newCol = tex2D(_ColorPaletteOriginal, float2(x/_textureWidth, .5));
					if (currentColor.r == newCol.r &&  currentColor.g == newCol.g && currentColor.b == newCol.b){// && currentColor.a == newCol.a){
						pos = x/_textureWidth;
					}
				}
				//_floatTest = _floatTest/64;
				//float4 ne = tex2D(_ColorPaletteOriginal, float2(_floatTest, .5));
				float4 result = tex2D(_ColorPaletteMorph, fixed2(pos, .5));
				result.a = currentColor.a;
				return result;



				//float4 result;
				//result.rgb = tex2D(_ColorPalette, float2(rColor, .5)).rgb;
				//result.a = tex2D(_MainTex, IN.texcoord.xy).a;
				//return result;
			}
		ENDCG
		}
	}
}