using UnityEngine;

namespace Feature.Common.Blur
{
    public class KawaseBlurEffect
    {
        private readonly Material _blurMaterial;
        private readonly int _iterations;

        public KawaseBlurEffect(Shader blurShader, int iterations = 4)
        {
            _blurMaterial = new Material(blurShader);
            _iterations = iterations;
        }

        public RenderTexture Blur(RenderTexture source)
        {
            var current = source;

            for (int i = 0; i < _iterations; i++)
            {
                var temp = RenderTexture.GetTemporary(current.width, current.height, 0, current.format);
                _blurMaterial.SetFloat("_Offset", 1f + i * 0.5f); // прогрессивно увеличиваем радиус

                Graphics.Blit(current, temp, _blurMaterial);

                if (current != source)
                    RenderTexture.ReleaseTemporary(current);

                current = temp;
            }

            return current;
        }
    }
}