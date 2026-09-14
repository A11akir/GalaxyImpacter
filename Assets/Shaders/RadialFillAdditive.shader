Shader "Custom/RadialFill"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FillAmount ("Fill Amount", Range(0, 1)) = 0.5
        _GlowColor ("Glow Color", Color) = (1, 0, 1, 1)
        _GlowIntensity ("Glow Intensity", Range(1, 20)) = 3
        _FadeLength ("Fade Length", Range(0.01, 1)) = 0.3
        _HDRMultiplier ("HDR Multiplier", Range(1, 10)) = 3
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="UniversalPipeline" }
        Blend SrcAlpha One
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
            float4 _GlowColor;
            float _GlowIntensity;
            float _FadeLength;
            float _HDRMultiplier;

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

                // угол 0..1 сверху по часовой
                float angle = atan2(uv.x, uv.y) / (6.28318) + 0.5;

                // обрезаем по fillAmount
                float fill = step(angle, _FillAmount);

                // градиент: 0 у стрелки, 1 далеко позади
                float distFromEdge = (_FillAmount - angle) / max(_FadeLength, 0.001);
                float fadeAlpha = saturate(distFromEdge);

                // форма руны
                float shape = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv).a;

                // свечение ярче у стрелки
                float glowBoost = 1.0 + (1.0 - fadeAlpha) * _GlowIntensity;

                // HDR цвет — больше 1.0 триггерит Bloom в URP
                float3 color = _GlowColor.rgb * glowBoost * _HDRMultiplier;

                float alpha = shape * fill * fadeAlpha * _GlowColor.a;

                return half4(color, alpha);
            }
            ENDHLSL
        }
    }
}