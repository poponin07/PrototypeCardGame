using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;

[CreateAssetMenu(fileName = "WarriorAbility", menuName = "Abilities/WarriorAbility")]
public class WarriorAbility : BaseAbilities
{
        [SerializeField] private CardManager m_cardManager;
        
        public override void ApplyAbility()
        {
            
        }
}
