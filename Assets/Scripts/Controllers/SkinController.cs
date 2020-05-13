using UnityEngine;

namespace Controllers
{
    public class SkinController : MonoBehaviour
    {
        public static string ButtonPressed;
        private Material[] _skins;
        private Material[] _mats;

        private void Start()
        {
            _skins = Resources.LoadAll<Material>("Materials");
        }

        // Update is called once per frame
        void Update()
        {
            switch (ButtonPressed)
            {
                case "1":
                    ButtonPressed = "";
                    ChangeSkin(0);
                    break;
                case "2":
                    ButtonPressed = "";
                    ChangeSkin(1);
                    break;
                case "3":
                    ButtonPressed = "";
                    ChangeSkin(2);
                    break;
                case "4":
                    ButtonPressed = "";
                    ChangeSkin(3);
                    break;
                case "5":
                    ButtonPressed = "";
                    ChangeSkin(4);
                    break;
                case "6":
                    ButtonPressed = "";
                    ChangeSkin(5);
                    break;
                case "7":
                    ButtonPressed = "";
                    ChangeSkin(6);
                    break;
                case "8":
                    ButtonPressed = "";
                    ChangeSkin(7);
                    break;
                case "9":
                    ButtonPressed = "";
                    ChangeSkin(8);
                    break;
            }
        }

        private void ChangeSkin(int num)
        {
            _mats = GetComponent<Renderer>().materials;
            _mats[0] = _skins[num];
            GetComponent<Renderer>().materials = _mats;
        }
    }
}
