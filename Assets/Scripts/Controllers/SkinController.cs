using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    //TODO: terrible buttons and planets naming allocating system atm. should be reworked really.
    //4example: new instance of skincontroller on other scene would be empty and the one trying
    //    to restore it would suffer a lot (I DID). 
    public class SkinController : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] buttons;
        [SerializeField]
        private string[] planetNames;
    
        public static string ButtonPressed;

        private static int _price1= 100;
        private static int _price2= 200;
        private static int _price3= 400;
        private static int _price4= 600;
        private static int _price5= 1000;
        private Material[] _skins;
        private Sprite[] _sprites;
        private Material[] _mats;
        private List<bool> _purchasedSkins;

        private void Start()
        {
            _skins = Resources.LoadAll<Material>("Materials/Planets");
            _sprites = Resources.LoadAll<Sprite>("Textures/Planets");
            ChangeSkin(SaveController.Instance.Save.PlayerSkin);
            _purchasedSkins = SaveController.Instance.Save.PurchasedSkins;
            for (int i = 0; i < _purchasedSkins.Count; i++)
            {
                if (_purchasedSkins[i])
                {
                    buttons[i].GetComponent<Image>().sprite = _sprites[i];
                    SetPlanetName(i);
                }
            }
        
        }
        
        //TODO: looks bad, probably it could be rewrote
        public void Update()
        {
            int i;
            switch (ButtonPressed)
            {
                case "0":
                    ButtonPressed = "";
                    ChangeSkin(0);
                    break;
                case "1":
                    i = 1;
                    ButtonPressed = "";
                    ClickOnSkinButton(i);
                    break;
                case "2":
                    i = 2;
                    ButtonPressed = "";
                    ClickOnSkinButton(i);
                    break;
                case "3":
                    i = 3;
                    ButtonPressed = "";
                    ClickOnSkinButton(i);
                    break;
                case "4":
                    i = 4;
                    ButtonPressed = "";
                    ClickOnSkinButton(i);
                    break;
                case "5":
                    i = 5;
                    ButtonPressed = "";
                    ClickOnSkinButton(i);
                    break;
                case "6":
                    i = 6;
                    ButtonPressed = "";
                    ClickOnSkinButton(i);
                    break;
                case "7":
                    i = 7;
                    ButtonPressed = "";
                    ClickOnSkinButton(i);
                    break;
                case "8":
                    i = 8;
                    ButtonPressed = "";
                    ClickOnSkinButton(i);
                    break;
            }
        }

        private void ClickOnSkinButton(int button)
        {
            if (!_purchasedSkins[button])
            {
                if (AbleToBuy(button))
                    BuySkin(button);
            }
            else
                ChangeSkin(button);
        }

        private bool AbleToBuy(int num)
        {
            var coins = SaveController.Instance.Save.CoinsCount;
            var cost = 0;
            
            if (num < 3)
                cost = _price1;
            else if (num < 6)
                cost = _price2;
            else if (num == 6)
                cost = _price3;
            else if (num == 7)
                cost = _price4;
            else
                cost = _price5;
            if (coins < cost)
                return false;
            GameController.Instance.AddCoins(-cost, GameObject.Find("CoinsAmountText").GetComponent<Text>());
            return true;
        }

        private void BuySkin(int num)
        {
            // TODO animation
            _purchasedSkins[num] = true;
            SaveController.Instance.Save.PurchasedSkins = _purchasedSkins;
            buttons[num].GetComponent<Image>().sprite = _sprites[num];
            SetPlanetName(num);
            SaveController.Instance.SaveGameToFile();
        }

        private void ChangeSkin(int num)
        {
            _mats = GetComponent<Renderer>().materials;
            _mats[0] = _skins[num];
            GetComponent<Renderer>().materials = _mats;
            SaveController.Instance.Save.PlayerSkin = num;
            SaveController.Instance.SaveGameToFile();
        }

        private void SetPlanetName(int num)
        {
            GameObject obj = buttons[num].transform.GetChild(0).gameObject;
            obj.GetComponent<Text>().text = planetNames[num];
        }
    }
}
