using Controllers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UiScripts
{
    public class ChangeSkinButton : MonoBehaviour, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            SkinController.ButtonPressed = EventSystem.current.currentSelectedGameObject.name;
        }
    }
}
