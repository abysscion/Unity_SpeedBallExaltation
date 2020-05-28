using Controllers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UiScripts
{
    public class BackButtonScript : MonoBehaviour, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            GameController.CurrentGameState = GameController.GameState.StartGame;
        }
    }
}
