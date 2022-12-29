
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class PlayerHand : MonoBehaviour
    {
        [SerializeField] private Players m_move;
        [SerializeField] private Transform[] m_positonsCardInHand;

        [SerializeField]
        private CardManager m_cardManger;
        private Card[] m_cardInHand1;
        private Card[] m_cardInHand2;
        private List<Card> m_cardOnTable;
        
        public Transform axis;
        private void Start()
        {
            m_cardInHand1 = new Card[m_positonsCardInHand.Length];
            m_cardInHand2 = new Card[m_positonsCardInHand.Length];
            m_cardOnTable = new List<Card>();
        }

        public bool SetNewCardInHand(Card newCard)
        {
            Card[] cardInHand;
            if (RoundManager.instance.PlayerMove != Players.Player1)
                cardInHand = m_cardInHand2;
                else
            {
                cardInHand = m_cardInHand1;
            }
            
            int result = GetIndexLastCard(cardInHand);

            if (result == -1)
            {
                Debug.LogWarning("Maximum number of cards in a hand");
                return false;
            }

            cardInHand[result] = newCard;
            newCard.StartCoroutine(newCard.LiftCard(newCard, m_positonsCardInHand[result]));

            return true;
        }

        private int GetIndexLastCard(Card[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == null)
                {
                    return i;
                }
            }
            return -1;
        }

        public void RemoveCardFromHand(Card removeCard)
        {
            Card[] m_cardInHand;
            if (RoundManager.instance.PlayerMove == Players.Player1)
            {
                m_cardInHand = m_cardInHand1;
            }
            else
            {
                m_cardInHand = m_cardInHand2;
            }
            
            for (int i = 0; i < m_cardInHand.Length; i++)
            {
                if (m_cardInHand[i] == removeCard)
                {
                    m_cardInHand[i] = null;
                    Debug.Log("All card is removed from the hand");
                    return;
                }
            } 
        }
        
        public bool AddCardOnTable(Card moveCard)
        {
            SlotScript slotScript = m_cardManger.GetClosestSlot(moveCard, true);
            var slotTransform = slotScript.transform;

            if (slotScript != null && !slotScript.couple &&  Vector3.Distance(moveCard.transform.position, slotTransform.position) < DragAndDropScript.MAGNET_RADIUS)
            {
                slotScript.SwitchCouple(moveCard);
                moveCard.m_curParent = slotTransform;
                moveCard.transform.SetParent(slotTransform);
                moveCard.transform.position = slotTransform.position;
                moveCard.m_cardState = CardState.OnTable;
                return false;
            }
            moveCard.StartCoroutine(moveCard.MoveInHandOrTable(moveCard, moveCard.m_curParent, CardState.InHand));
            return true;
        }

        public bool CardAttack(Card moveCard)
        {
            SlotScript slotScript = m_cardManger.GetClosestSlot(moveCard, false);
            AnimationComponent animationComponent = moveCard.GetComponent<AnimationComponent>();
            var slotTransform = slotScript.transform;
            
            if (slotScript != null && slotScript.couple && Vector3.Distance(moveCard.transform.position, slotTransform.position) < DragAndDropScript.MAGNET_RADIUS)
            {
                animationComponent.AnimationShakeCard();
                
               bool attackResult = slotScript.GetCardCouple().GetDamage(moveCard.m_attack);

               if (attackResult)
               {
                   slotScript.SwitchCouple(slotScript.GetCardCouple());
               }
                
                moveCard.transform.position = slotTransform.position;

                moveCard.StartCoroutine(moveCard.MoveInHandOrTable(moveCard, moveCard.m_curParent, CardState.OnTable));
                
                return false;
            }
            moveCard.StartCoroutine(moveCard.MoveInHandOrTable(moveCard, moveCard.m_curParent, CardState.OnTable));
            
            return true;
        }
    }
    }
