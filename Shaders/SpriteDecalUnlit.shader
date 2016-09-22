Shader "Decal2D/SpriteDecalUnlit"
{
	Properties
	{
	    _Color ("Tint", Color) = (1,1,1,1)
		_DecalColor("DecalColor", Color) = (1,1,1,1)
		_MainTex ("Sprite Texture", 2D) = "white" {}
		_DecalTex ("Decal Texture", 2D) = "black" {}
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
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 uv_texcoord : TEXCOORD0;
				float2 uv2_texcoord : TEXCOORD1;
		    };

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 uv_texcoord  : TEXCOORD0;
				half2 uv2_texcoord : TEXCOORD1;
			};
			
			fixed4 _Color, _DecalColor;

			v2f vert(appdata_t i)
			{
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				o.vertex = mul(UNITY_MATRIX_MVP, i.vertex);
				o.uv_texcoord = i.uv_texcoord;
				o.uv2_texcoord = i.uv2_texcoord;
				return o;
			}

			uniform sampler2D _MainTex, _DecalTex;
			uniform float4 _MainTex_ST, _DecalTex_ST;

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = tex2D(_MainTex, _MainTex_ST.xy * IN.uv_texcoord.xy + _MainTex_ST.zw) * _Color;
				fixed4 dec = tex2D(_DecalTex, _DecalTex_ST.xy * IN.uv2_texcoord.xy + _DecalTex_ST.zw);
				dec *= _DecalColor;
			//	fixed4 b = min(c, dec);
				c.rgb = lerp(c.rgb, dec.rgb, dec.a);
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
}
