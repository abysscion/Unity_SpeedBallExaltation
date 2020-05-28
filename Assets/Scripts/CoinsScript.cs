using Controllers;
using UnityEngine;
using UnityEngine.UI;

public class CoinsScript : MonoBehaviour
{
    private GameObject _coinsText;
    private Vector3 _coinsTextPosition;
    private Vector3 _direction;
    private bool _isAlreadyDestroying;

    private void Start()
    {
        _coinsText = GameObject.Find("CoinsAmountText");
    }

    private void Update()
    {
        if (transform.position.y > _coinsText.transform.position.y)
            Destroy(this.gameObject);
        if (!_isAlreadyDestroying)
        {
            _isAlreadyDestroying = !_isAlreadyDestroying;
            _direction = _coinsText.GetComponent<RectTransform>().position - GetComponent<RectTransform>().position;
        }
        
        transform.Translate(_direction * Time.deltaTime);
    }

    private void OnDestroy()
    {
        GameController.Instance.AddCoins(1, _coinsText.GetComponent<Text>());
    }
}
