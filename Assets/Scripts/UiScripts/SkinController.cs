using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinController : MonoBehaviour
{
    public static string ButtonPressed;
    
    public static SkinController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.Log("[ATTENTION] Multiple " + this + " found!");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (ButtonPressed)
        {
            case "1":
                Debug.Log("YEAH BOY!");
                ButtonPressed = "";
                break;
            case "2":
                ButtonPressed = "";
                break;
            case "3":
                ButtonPressed = "";
                break;
            case "4":
                ButtonPressed = "";
                break;
            case "5":
                ButtonPressed = "";
                break;
            case "6":
                ButtonPressed = "";
                break;
            case "7":
                ButtonPressed = "";
                break;
            case "8":
                ButtonPressed = "";
                break;
            case "9":
                ButtonPressed = "";
                break;
        }
    }
}
