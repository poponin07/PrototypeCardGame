using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    //повышение статов всех миньонов
    [CreateAssetMenu(fileName = "AllSummonedUpStats", menuName = "Effects/SummonedUpStats")]
    public class SummonedUpStats : BaseEffect
    {
        [SerializeField] private int damage;
        [SerializeField] private int health;
        private List<Card> effectedCards;

        public override void ApplyEffect(CardManager cardManager, Card effectOwner)
        {
            effectedCards = cardManager.SummonedCardsAddStats(damage, health);
        }

        public override bool TryToRemoveEffect(CardManager cardManager)
        {
            foreach (var card in effectedCards)
            {
                if (card != null)
                {
                    card.Attack -= damage;
                    card.Health -= health; 
                }
                
            }

            return true;
        }
    }
}