using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;

[CreateAssetMenu(fileName = "PriestAbility", menuName = "Abilities/PriestAbility")]
public class PriestAbility : BaseAbilities
{
    [SerializeField] private int m_healValue;

    public override void ApplyAbility()
    {
        if (m_cardManager == null)
        { 
            m_cardManager = FindObjectOfType<CardManager>();      
        }

        m_cardManager.RestoreHealthCharacters(m_healValue);
    }
}
