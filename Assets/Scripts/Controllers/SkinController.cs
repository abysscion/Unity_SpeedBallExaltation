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
        public static string ButtonPressed;

        [SerializeField] private GameObject[] buttons;
        [SerializeField] private string[] planetNames;
        private Material[] _skins;
        private Material[] _mats;
        private Sprite[] _sprites;
        private List<bool> _purchasedSkins;
        private const int Price1 = 100;
        private const int Price2 = 200;
        private const int Price3 = 400;
        private const int Price4 = 600;
        private const int Price5 = 1000;

        private void Start()
        {
            _skins = Resources.LoadAll<Material>("Materials/Planets");
            _sprites = Resources.LoadAll<Sprite>("Textures/Planets");
            _purchasedSkins = SaveController.Instance.Save.PurchasedSkins;
            ChangeSkin(SaveController.Instance.Save.PlayerSkin);
            for (var i = 0; i < _purchasedSkins.Count; i++)
            {
                if (!_purchasedSkins[i]) continue;
                buttons[i].GetComponent<Image>().sprite = _sprites[i];
                SetPlanetName(i);
            }
        }
        
        //TODO: switches with identical lines looks bad, probably it could be rewrote simpler
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
            int cost;
            
            if (num < 3)
                cost = Price1;
            else if (num < 6)
                cost = Price2;
            else if (num == 6)
                cost = Price3;
            else if (num == 7)
                cost = Price4;
            else
                cost = Price5;
            if (coins < cost)
                return false;
            GameController.Instance.AddCoins(-cost, GameObject.Find("CoinsAmountText").GetComponent<Text>());
            return true;
        }

        private void BuySkin(int num)
        {
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
            var obj = buttons[num].transform.GetChild(0).gameObject;
            
            obj.GetComponent<Text>().text = planetNames[num];
        }
    }
}
