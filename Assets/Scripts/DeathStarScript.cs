using System;
using UnityEngine;

public class DeathStarScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            GameController.currentGameState = GameController.GameState.Lose;
        }
    }
}
