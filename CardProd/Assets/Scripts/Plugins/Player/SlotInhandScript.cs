using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotInhandScript : MonoBehaviour
{
    [SerializeField]private bool isSwapedCardOnFerstRound;
    
    public void SetIsSwapedCardOnFerstRound()
    {
        isSwapedCardOnFerstRound = true;
    }


    public bool GetIsSwapedCardOnFerstRound()
    {
        return isSwapedCardOnFerstRound;
    }
        
}
