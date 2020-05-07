using System.Collections.Generic;
using UnityEngine;

public class AsteroidExploder : MonoBehaviour
{
    public float destroyTimer = 1.0f;

    private List<Collider> childrenColliders;

    private void Start()
    {
        childrenColliders = new List<Collider>();

        foreach (var collider in GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
            childrenColliders.Add(collider);
        }
        childrenColliders[0].enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        var jumper = other.gameObject.GetComponent<JumpController>();
        var jumperRb = jumper.GetComponent<Rigidbody>();

        // if (jumper.isStick)
        // {
        //     jumper.UnstickBall();
        //     jumperRb.velocity = Vector3.down;
        // }
        Destroy(this.gameObject, destroyTimer);
        foreach (var collider in GetComponentsInChildren<Collider>())
            collider.enabled = true;
    }
}
