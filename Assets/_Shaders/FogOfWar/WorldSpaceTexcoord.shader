Shader "Custom/WorldSpaceTexcoord" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _MaskRes ("Mask Resolution", Float) = 512.0
    }
    SubShader {
    	Tags {"Queue"="Transparent" "RenderType"="Transparent"}
    	Blend SrcAlpha OneMinusSrcAlpha
        Pass {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            
			struct v2f {
				float4 pos : SV_POSITION;
				float4 uv : TEXCOORD0;
			};
            
            uniform sampler2D _MainTex;
            uniform float _MaskRes;
            
			v2f vert (appdata_base v)
			{
				v2f o;
				float4 worldPos = mul(_Object2World, v.vertex);
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.uv = float4( worldPos.xz / _MaskRes, 0, 0 );
				return o;
			}

            fixed4 frag(v2f i) : SV_Target {
            	float4 c = tex2D(_MainTex, i.uv);
                return c;
            }

            ENDCG
        }
    }
}