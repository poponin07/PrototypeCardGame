using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;
using Player;

[CreateAssetMenu(fileName = "HunterAbility", menuName = "Abilities/HunterAbility")]
public class HunterAbility : BaseAbilities
{
    [SerializeField] private int m_damage;

    public override void ApplyAbility()
    {
        if (m_cardManager == null)
        { 
            m_cardManager = FindObjectOfType<CardManager>();      
        }

        PlayerScript playerScript = RoundManager.instance.PlayerMove == Players.Player1
            ? m_cardManager.player1Script
            : m_cardManager.player2Script;
        
        playerScript.GetDamage(m_damage, false);
    }
}
