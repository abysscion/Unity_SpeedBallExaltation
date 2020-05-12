using System;
using System.Collections;
using System.Collections.Generic;
using UiScripts;
using UnityEngine;
using UnityEngine.UI;

public class SkinController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] buttons;
    [SerializeField]
    private string[] planetNames;
    
    public static string ButtonPressed;

    public int skinPrice = 100;
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

    // Update is called once per frame
    void Update()
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
            {
                BuySkin(button);
            }
            else
                return;
        }
        else
            ChangeSkin(button);
    }

    private bool AbleToBuy(int num)
    {
        if (SaveController.Instance.Save.CoinsCount > skinPrice)
        {
            return true;
        }

        return false;
    }

    private void BuySkin(int num)
    {
        // TODO animation
        _purchasedSkins[num] = true;
        SaveController.Instance.Save.PurchasedSkins = _purchasedSkins;
        buttons[num].GetComponent<Image>().sprite = _sprites[num];
        SetPlanetName(num);
        GameController.Instance.AddCoins(-skinPrice);
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
