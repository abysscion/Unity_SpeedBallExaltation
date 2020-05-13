using UnityEngine;

namespace Utilities
{
    public class BackgroundParallaxScript : MonoBehaviour
    {
        public float parallaxSpeed = 1;

        private const float DefaultOffsetChange = -0.001f;
        private Material _backgroundMaterial;

        private void Start()
        {
            _backgroundMaterial = this.GetComponent<Renderer>().material;
        }

        private void Update()
        {
            var newOffset = _backgroundMaterial.mainTextureOffset;

            if (Mathf.Abs(newOffset.y) >= 1.0f)
                newOffset.y = 0.0f;
            newOffset.y += parallaxSpeed * DefaultOffsetChange * Time.deltaTime;
            _backgroundMaterial.mainTextureOffset = newOffset;
        }
    }
}
