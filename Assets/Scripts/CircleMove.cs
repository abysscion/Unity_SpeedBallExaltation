using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMove : MonoBehaviour
{
    public float rotateSpeed = 1f;
    public float radius = 2f;
    public Vector3 centre = new Vector3(0.0f, 1.5f, 0.0f);
    
    private float _angle;

    private void Update()
    {
        _angle += rotateSpeed * Time.deltaTime;
        var offset = new Vector3(Mathf.Sin(_angle), 0.0f, Mathf.Cos(_angle)) * radius;
        transform.position = centre + offset;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(centre, 0.1f);
        Gizmos.DrawLine(centre, transform.position);
    }
}
