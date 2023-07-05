using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;

[CreateAssetMenu(fileName = "MagicalAbility", menuName = "Abilities/MagicalAbility")]
public class Magicanability : BaseAbilities
{
        [SerializeField] private int m_damage;

        public override void ApplyAbility()
        {
                if (m_cardManager == null)
                { 
                        m_cardManager = FindObjectOfType<CardManager>();      
                }
                
                m_cardManager.DealDamage(m_damage);
        }
}
