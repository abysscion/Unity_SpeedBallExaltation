using Controllers;
using UnityEngine;

public class FinishScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            GameController.CurrentGameState = GameController.GameState.Win;
        }
    }
}
