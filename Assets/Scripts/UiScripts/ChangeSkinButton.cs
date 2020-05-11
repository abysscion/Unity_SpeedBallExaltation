using UnityEngine;
using UnityEngine.EventSystems;

namespace UiScripts
{
    public class ChangeSkinButton : MonoBehaviour, IPointerDownHandler
    {
        public string myName;
        
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
            myName = EventSystem.current.currentSelectedGameObject.name;
            SkinController.ButtonPressed = myName;
        }
    }
}
