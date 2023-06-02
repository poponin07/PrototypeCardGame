using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    //повышение урона миньонов-мурлоков
    [CreateAssetMenu(fileName = "SummonedMurlocsDamageStats", menuName = "Effects/SummonedMurlocsDamageStats")]
    public class SummonedMurlocsDamageStats : BaseEffect
    {
        [SerializeField] private int damage;
        private List<Card> effectedCards;

        public override void ApplyEffect(CardManager cardManager, Card effectOwner)
        {
            effectedCards = cardManager.SummonedMurlocsAddDamage(damage);
        }

        public override bool TryToRemoveEffect(CardManager cardManager)
        {
            foreach (var card in effectedCards)
            {
                card.Attack -= damage;
            }

            return true;
        }
    }
}