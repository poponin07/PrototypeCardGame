using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(fileName = "BattleCryDealDamage", menuName = "Effects/BattleCryDealDamage")]
    public class BattleCryDealDamage : BaseEffect
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