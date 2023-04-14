using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cards;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Cards
{
    [Serializable]
    public abstract class BaseEffect : ScriptableObject
    {
        public abstract void ApplyEffect(CardManager cardManager);

        public abstract bool TryToRemoveEffect();
    }


    [CreateAssetMenu(fileName = "StatsEffect", menuName = "Effects/StatsEffect")]
    public class StatsEffect : BaseEffect
    {
        public int Damage;
        public int Health;
        [NonSerialized] private Card m_effectedCard;

        public override void ApplyEffect(CardManager cardManager)
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

        public override bool TryToRemoveEffect()
        {
            m_effectedCard.Health -= Health;
            m_effectedCard.Attack -= Damage;

            return true;
        }
    }

    [CreateAssetMenu(fileName = "BattleCry", menuName = "Effects/BattleCry")]
    public class BattleCry : BaseEffect
    {
        public override void ApplyEffect(CardManager cardManager)
        {
        }

        public override bool TryToRemoveEffect()
        {
            throw new System.NotImplementedException();
        }
    }
    
    [CreateAssetMenu(fileName = "BattleCryDealDamage", menuName = "Effects/BattleCryDealDamage")]
    public class BattleCryDealDamage : BaseEffect
    {
        [SerializeField] private int damage;
        public override void ApplyEffect(CardManager cardManager)
        {
            cardManager.DealDamage(damage);
        }
        public override bool TryToRemoveEffect()
        {
            return true;
        }
    }
    
    [CreateAssetMenu(fileName = "BattleCryRestoreHealth", menuName = "Effects/BattleCryRestoreHealth")]
    public class BattleCryRestoreHealth : BaseEffect
    {
        public int valueHealthRestore;
        public override void ApplyEffect(CardManager cardManager)
        {
            if (RoundManager.instance.PlayerMove == Players.Player1)
            {
                cardManager.player1Script.RestoreHealth(valueHealthRestore);
            }
            else
            {
                cardManager.player2Script.RestoreHealth(valueHealthRestore);
            }
        }

        public override bool TryToRemoveEffect()
        {
            return true;
        }
    }

    [CreateAssetMenu(fileName = "Summon", menuName = "Effects/Summon")]
    public class Summon : BaseEffect
    {
        public int cardIdToSummon;
        [NonSerialized] private Card m_effectOwner;

        public override void ApplyEffect(CardManager cardManager)
        {
            
            cardManager.AddSummonCardOntable(cardIdToSummon);
            
        }

        public override bool TryToRemoveEffect()
        {
            return true;
        }
    }
}