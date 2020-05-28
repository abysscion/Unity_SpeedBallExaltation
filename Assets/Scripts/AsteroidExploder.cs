using System.Collections.Generic;
using Controllers;
using UnityEngine;

public class AsteroidExploder : MonoBehaviour
{
    public float destroyTimer = 1.0f;

    private List<Collider> _childrenColliders;
    private bool _alreadyEnter;

    private void Start()
    {
        _childrenColliders = new List<Collider>();
        foreach (var cld in GetComponentsInChildren<Collider>())
        {
            cld.enabled = false;
            _childrenColliders.Add(cld);
        }
        _childrenColliders[0].enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || _alreadyEnter)
            return;
        
        var coin = Resources.Load<GameObject>("Prefabs/Coin2d");
        
        _alreadyEnter = true;
        SoundController.Instance.PlaySound(SoundController.SoundName.AsteroidExplosion);
        coin = Instantiate(coin, Camera.main.WorldToScreenPoint(other.transform.position), Quaternion.identity);
        coin.transform.SetParent(GameObject.Find("UI").transform, true);
        Destroy(this.gameObject, destroyTimer);
        foreach (var cld in GetComponentsInChildren<Collider>())
            cld.enabled = true;
    }
}
