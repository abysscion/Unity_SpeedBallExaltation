using System.Collections.Generic;
using Controllers;
using UnityEngine;
using Text = UnityEngine.UI.Text;

public class AsteroidExploder : MonoBehaviour
{
    public float destroyTimer = 1.0f;

    private List<Collider> _childrenColliders;
    private Text _txtComponent;
    private bool _alreadyEnter = false;

    private void Start()
    {
        _childrenColliders = new List<Collider>();

        foreach (var cld in GetComponentsInChildren<Collider>())
        {
            cld.enabled = false;
            _childrenColliders.Add(cld);
        }
        _childrenColliders[0].enabled = true;
        _txtComponent = GameObject.Find("CoinsAmountText").GetComponent<Text>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || _alreadyEnter) return;

        _alreadyEnter = true;
        //TODO монетки должны вылетать из метеорита, а не из центра экрана
        GameObject canvas = GameObject.Find("UI");
        GameObject coin = Resources.Load<GameObject>("Prefabs/Coin2d");
        coin = Instantiate(coin, transform.position, Quaternion.identity);
        coin.transform.SetParent(canvas.transform, false);
        Destroy(this.gameObject, destroyTimer);
        foreach (var cld in GetComponentsInChildren<Collider>())
            cld.enabled = true;
    }
}
