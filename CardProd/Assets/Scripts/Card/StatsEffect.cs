using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Cards
{
    [CreateAssetMenu(fileName = "StatsEffect", menuName = "Effects/StatsEffect")]
    public class StatsEffect : BaseEffect
    {
        public int Damage;
        public int Health;
        [NonSerialized] private Card m_effectedCard;

        public override void ApplyEffect(CardManager cardManager, Card effectOwner)
        {
            List<Card> targetCards = new List<Card>();
            targetCards.AddRange(
                RoundManager.instance.PlayerMove == Players.Player1
                    ? cardManager.cardsPlayedPlayer1
                    : cardManager.cardsPlayedPlayer2);
            int rand = Random.Range(0, targetCards.Count);
            m_effectedCard = targetCards[rand];
            m_effectedCard.Health += Health;
            m_effectedCard.Attack += Damage;
        }

        public override bool TryToRemoveEffect(CardManager cardManager)
        {
            m_effectedCard.Health -= Health;
            m_effectedCard.Attack -= Damage;

            return true;
        }
    }
}