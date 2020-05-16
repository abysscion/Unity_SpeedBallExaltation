using Controllers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UiScripts
{
    public class BallChooseMenu : MonoBehaviour, IPointerDownHandler
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            GameController.CurrentGameState = GameController.GameState.ChooseBall;
        }
    }
}
