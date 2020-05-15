using System.Collections.Generic;
using Controllers;
using UnityEngine;
using Text = UnityEngine.UI.Text;

public class AsteroidExploder : MonoBehaviour
{
    public float destroyTimer = 1.0f;

    private List<Collider> childrenColliders;
    private Text _txtComponent;

    private void Start()
    {
        childrenColliders = new List<Collider>();

        foreach (var cld in GetComponentsInChildren<Collider>())
        {
            cld.enabled = false;
            childrenColliders.Add(cld);
        }
        childrenColliders[0].enabled = true;
        _txtComponent = GameObject.Find("CoinsAmountText").GetComponent<Text>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        GameController.Instance.AddCoins(1, _txtComponent);
        Destroy(this.gameObject, destroyTimer);
        foreach (var cld in GetComponentsInChildren<Collider>())
            cld.enabled = true;
    }
}
