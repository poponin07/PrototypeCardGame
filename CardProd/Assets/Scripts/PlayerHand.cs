using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class PlayerHand : MonoBehaviour
    {
        [SerializeField] private Transform[] m_positonsCardInHand;
        private Card[] m_cardInHand;

        private void Start()
        {
            m_cardInHand = new Card[m_positonsCardInHand.Length];
        }

        public bool SetNewCard(Card newCard)
        {
            var result = GetIndexLastCardInHand();

            if (result == -1)
            {
                Debug.LogWarning("Maximum number of cards in a hand");
                return false;
            }

            m_cardInHand[result] = newCard;

            newCard.StartCoroutine(newCard.LiftCard(newCard, m_positonsCardInHand[result]));

            return true;
        }

        private int GetIndexLastCardInHand()
        {
            for (int i = 0; i < m_cardInHand.Length; i++)
            {
                if (m_cardInHand[i] == null)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}

