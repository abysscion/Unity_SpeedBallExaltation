using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float xRotator;
    public float yRotator;
    public float zRotator;
    public bool xLock;
    public bool yLock;
    public bool zLock;

    private void Start()
    {
        xRotator = xRotator <= 0.0f ? Random.Range(0, 90) : xRotator;
        yRotator = yRotator <= 0.0f ? Random.Range(0, 90) : yRotator;
        zRotator = zRotator <= 0.0f ? Random.Range(0, 90) : zRotator;
    }

    private void Update()
    {
        this.transform.Rotate(xLock ? 0 : xRotator * Time.deltaTime,
            yLock ? 0 : yRotator * Time.deltaTime, 
            zLock ? 0 : zRotator * Time.deltaTime);
    }
}
