using System;
using UnityEngine;

namespace Cards
{
    [Serializable]
    public abstract class BaseEffect : ScriptableObject
    {
        public abstract void ApplyEffect(CardManager cardManager, Card effectOwner);

        public abstract bool TryToRemoveEffect(CardManager cardManager);
    }

    //[CreateAssetMenu(fileName = "BattleCry", menuName = "Effects/BattleCry")]
    /*public class BattleCry : BaseEffect
    {
        public override void ApplyEffect(CardManager cardManager, Card effectOwner)
        {
        }

        public override bool TryToRemoveEffect(CardManager cardManager)
        {
            return true;
        }
    }*/
    
    [CreateAssetMenu(fileName = "BattleCryDealDamagePlayer", menuName = "Effects/BattleCryDealDamagePlayer")]
    public class BattleCryDealDamagePlayer : BaseEffect
    {
        [SerializeField] private int damage;

        public override void ApplyEffect(CardManager cardManager, Card effectOwner)
        {
            cardManager.DealDamage(damage);
        }

        public override bool TryToRemoveEffect(CardManager cardManager)
        {
            return true;
        }
    }
}