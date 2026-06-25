using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
namespace Feature.Common.Blur
{
    [RequireComponent(typeof(RawImage))]
    public class UIBlurBackground : MonoBehaviour
    {
        [SerializeField] private Shader _blurShader;
        [SerializeField] private Camera _captureCamera; // основная камера сцены
        [SerializeField] private int _iterations = 4;
        [SerializeField] private int _downsample = 2;
        [SerializeField] private RawImage _rawImage;
        
        private KawaseBlurEffect _blurEffect;
        private RenderTexture _captureTexture;
        
        [Button]
        public void CaptureAndBlur()
        {
            _blurEffect = new KawaseBlurEffect(_blurShader, _iterations);
            int width = Screen.width / _downsample;
            int height = Screen.height / _downsample;

            if (_captureTexture == null || _captureTexture.width != width)
            {
                if (_captureTexture != null) _captureTexture.Release();
                _captureTexture = new RenderTexture(width, height, 0);
            }

            var prevTarget = _captureCamera.targetTexture;
            _captureCamera.targetTexture = _captureTexture;
            _captureCamera.Render();
            _captureCamera.targetTexture = prevTarget;

            var blurred = _blurEffect.Blur(_captureTexture);
            _rawImage.texture = blurred;
        }
    }
}