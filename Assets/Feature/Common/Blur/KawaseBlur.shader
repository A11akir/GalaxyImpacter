Shader "Custom/KawaseBlur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Offset ("Offset", Float) = 1.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float _Offset;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 texel = _MainTex_TexelSize.xy * _Offset;

                fixed4 col = fixed4(0,0,0,0);
                col += tex2D(_MainTex, i.uv + float2( texel.x,  texel.y));
                col += tex2D(_MainTex, i.uv + float2(-texel.x,  texel.y));
                col += tex2D(_MainTex, i.uv + float2( texel.x, -texel.y));
                col += tex2D(_MainTex, i.uv + float2(-texel.x, -texel.y));

                return col * 0.25;
            }
            ENDCG
        }
    }
}