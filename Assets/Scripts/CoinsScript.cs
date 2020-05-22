using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;
using UnityEngine.UI;

public class CoinsScript : MonoBehaviour
{
    private Vector3 _coinsTextPosition;
    private GameObject _coinsText;
    private bool _isAlreadyDestroing = false;
    private Vector3 _direction;
    
    // Start is called before the first frame update
    void Start()
    {
        _coinsText = GameObject.Find("CoinsAmountText");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > _coinsText.transform.position.y)
            Destroy(this.gameObject);
        if (!_isAlreadyDestroing)
        {
            _isAlreadyDestroing = !_isAlreadyDestroing;
            _direction = _coinsText.GetComponent<RectTransform>().position - GetComponent<RectTransform>().position;
        }
        
        transform.Translate(_direction * Time.deltaTime);
    }

    private void OnDestroy()
    {
        GameController.Instance.AddCoins(1, _coinsText.GetComponent<Text>());
    }
}
