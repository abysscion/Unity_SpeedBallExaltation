using UnityEngine;

public class BackgroundParallaxScript : MonoBehaviour
{
    public float parallaxSpeed = 1;

    private const float DefaultOffsetChange = -0.001f;
    private Material bgrMat;

    private void Start()
    {
        bgrMat = this.GetComponent<Renderer>().material;
    }

    private void Update()
    {
        var newOffset = bgrMat.mainTextureOffset;

        if (Mathf.Abs(newOffset.y) >= 1.0f)
            newOffset.y = 0.0f;
        newOffset.y += parallaxSpeed * DefaultOffsetChange * Time.deltaTime;
        bgrMat.mainTextureOffset = newOffset;
    }
}
