using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
   public Toggle useCustomeDeck;

   private void Awake()
   {
       useCustomeDeck.onValueChanged.AddListener(ToggleOnValueSet);
       
       int value = PlayerPrefs.GetInt("UseDefDeck");

       if (value != null && value == 1)
       {
           useCustomeDeck.isOn = true;
       }
       else if (value != null && value != 1)
       {
           useCustomeDeck.isOn = false;
       }
       
   }


   private void ToggleOnValueSet(bool isOn)
    {
        if (useCustomeDeck.isOn)
        {
            PlayerPrefs.SetInt("UseDefDeck", 1);
        }
        else
        {
            PlayerPrefs.SetInt("UseDefDeck", 0);
        }
        
    }

    
}
