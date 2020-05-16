using Controllers;
using UnityEngine;

public class DeathStarScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            GameController.CurrentGameState = GameController.GameState.Lose;
        }
    }
}
