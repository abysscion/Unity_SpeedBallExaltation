using UnityEngine;

namespace Utilities
{
    public class BackgroundParallaxScript : MonoBehaviour
    {
        public float parallaxSpeedX = 0.0f;
        public float parallaxSpeedY = 1.0f;
        
        private const float DefaultOffsetChange = -0.001f;
        private Material _backgroundMaterial;

        private void Start()
        {
            _backgroundMaterial = this.GetComponent<Renderer>().material;
        }

        private void Update()
        {
            var newOffset = _backgroundMaterial.mainTextureOffset;

            if (Mathf.Abs(newOffset.x) >= 1.0f)
                newOffset.x = 0.0f;
            if (Mathf.Abs(newOffset.y) >= 1.0f)
                newOffset.y = 0.0f;
            if (parallaxSpeedX > 0.0f || parallaxSpeedX < 0.0f)
                newOffset.x += parallaxSpeedX * DefaultOffsetChange * Time.deltaTime;
            if (parallaxSpeedY > 0.0f || parallaxSpeedY < 0.0f)
                newOffset.y += parallaxSpeedY * DefaultOffsetChange * Time.deltaTime;
            
            _backgroundMaterial.mainTextureOffset = newOffset;
        }
    }
}
