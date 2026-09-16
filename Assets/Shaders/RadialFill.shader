Shader "Custom/RadialFill"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FillAmount ("Fill Amount", Range(0, 1)) = 0.5
        _ColorStart ("Color Start", Color) = (0.5, 0, 0.8, 1)
        _ColorEnd ("Color End", Color) = (1, 0, 1, 1)
        _FadeLength ("Fade Length", Range(0.01, 1)) = 0.3
        _GlowStrength ("Glow Strength", Range(0, 1)) = 1.0
        _AlphaFadeLength ("Alpha Fade Length", Range(0.01, 1)) = 0.3
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="UniversalPipeline" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            float _FillAmount;
            float4 _ColorStart;
            float4 _ColorEnd;
            float _FadeLength;
            float _GlowStrength;
            float _AlphaFadeLength;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float2 uv = IN.uv - 0.5;

                float angle = atan2(uv.x, uv.y) / (6.28318) + 0.5;
                float fill = step(angle, _FillAmount);

                float distFromEdge = (_FillAmount - angle) / max(_FadeLength, 0.001);
                float distFromEdgeAlpha = (_FillAmount - angle) / max(_AlphaFadeLength, 0.001);

                // градиент цвета — независимый от альфы
                float colorT = smoothstep(0.0, 1.0, saturate(distFromEdge));
                float3 color = lerp(_ColorEnd.rgb, _ColorStart.rgb, colorT);

                // альфа затухает у стрелки независимо от цвета
                float alphaFade = smoothstep(0.0, 1.0, saturate(distFromEdgeAlpha));

                float shape = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv).a;
                float alpha = shape * fill * alphaFade * _GlowStrength;

                return half4(color, alpha);
            }
            ENDHLSL
        }
    }
}