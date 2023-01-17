using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotInhandScript : MonoBehaviour
{
    [SerializeField]private bool isSwapedCardOnFerstRound;

    private void Start()
    {
       // isSwapedCardOnFerstRound = true;
    }

    public void SetIsSwapedCardOnFerstRound()
    {
        isSwapedCardOnFerstRound = false;
    }


    public bool GetIsSwapedCardOnFerstRound()
    {
        return isSwapedCardOnFerstRound;
    }
        
}
