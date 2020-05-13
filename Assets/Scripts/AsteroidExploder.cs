using System.Collections.Generic;
using Controllers;
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

        GameController.Instance.AddCoins(1);
        Destroy(this.gameObject, destroyTimer);
        foreach (var collider in GetComponentsInChildren<Collider>())
            collider.enabled = true;
    }
}
