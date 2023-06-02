using UnityEngine;

namespace Cards
{
    //повышение статов одного миньона
    [CreateAssetMenu(fileName = "SummonedUpStats", menuName = "Effects/SummoneUpStats")]
    public class SummoneUpStats : BaseEffect
    {
        [SerializeField] private int damage;
        [SerializeField] private int health;
        private Card effectedCard;

        public override void ApplyEffect(CardManager cardManager, Card effectOwner)
        {
            effectedCard = cardManager.SummoneCardAddStats(damage, health);
        }

        public override bool TryToRemoveEffect(CardManager cardManager)
        {
            effectedCard.Attack -= damage;
            effectedCard.Health -= health;
            
            return true;
        }
    }
}