Shader "Unlit/RopeShader1"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		//Tags { "RenderType"="Opaque" }
		LOD 100

		ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 tangent : TANGENT;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float3 worldPos : TEXCOORD1;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{	
				float t = i.uv.y * 2.0 - 1.0;

				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv + float2(_Time.x, 0.0)) + _Color;
				//fixed4 col = _Color;

				//Fade edges				
				float fade = smoothstep(0.0, 0.6, 1.0 - abs(t));
				col *= fade;

				//float stripe = smoothstep(0.1, 0.15 + 0.3 * (sin(i.uv.x * (50.0 + 25.0 * sin(_Time.x * 500.0))) * 0.5 + 0.5), abs(t));
				float stripe = smoothstep(0.05, 0.15 + 0.075 * sin(i.uv.x * 50.0) * sin(_Time.x * 200.0), abs(t));
				col.rgb *= stripe;

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
