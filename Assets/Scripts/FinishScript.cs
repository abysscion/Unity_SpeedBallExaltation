using UnityEngine;

public class FinishScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            // GameController.StartNextLevel();
            GameController.RestartLevel();
        }
    }
}
