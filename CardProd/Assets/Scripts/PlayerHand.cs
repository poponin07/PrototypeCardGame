
using System;
using System.Collections.Generic;
using Player;
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
        //private List<Card> m_cardOnTable;
        
        public Transform axis;
        private void Start()
        {
            m_cardInHand1 = new Card[m_positonsCardInHand.Length];
            m_cardInHand2 = new Card[m_positonsCardInHand.Length];
            //m_cardOnTable = new List<Card>();
        }
        
        
        public bool SetNewCardInHand(Card newCard, bool fromDeck)
        {
            Card[] cardInHand;
            if (RoundManager.instance.PlayerMove != Players.Player1)
            {
                cardInHand = m_cardInHand2;
            }
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
            if (fromDeck)
            {
                newCard.StartCoroutine(newCard.LiftCard(newCard, m_positonsCardInHand[result], fromDeck));  
                newCard.SetSlotInCurHand(m_positonsCardInHand[result].GetComponent<SlotInhandScript>());
            }
            else
            {
                newCard.StartCoroutine(newCard.MoveInHandOrTable(newCard, newCard.m_curParent, CardState.InHand));
            }
            return true;
        }

        public int GetIndexLastCard(Card[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == null)
                {
                    return i;
                }
                /*Debug.Log(arr[i].gameObject.transform.childCount);
                if (arr[i].gameObject.transform.childCount == 0)
                {
                    return i; 
                }*/
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
           
            if (slotScript != null && !slotScript.couple &&  Vector3.Distance(moveCard.transform.position, slotTransform.position) < DragAndDropScript.MAGNET_RADIUS && !slotScript.m_isPlyerAvatar && m_cardManger.CheckingCardRequirements(moveCard))
            {
                slotScript.SwitchCouple(moveCard);
                moveCard.m_curParent = slotTransform;
                moveCard.transform.SetParent(slotTransform);
                moveCard.transform.position = slotTransform.position;
                
                List<Card> arr = moveCard.players  == Players.Player1 ? m_cardManger.cardsPlayedPlayer1 : m_cardManger.cardsPlayedPlayer2;
                arr.Add(moveCard);
                
                moveCard.SwitchCardState(moveCard,CardState.OnTable);
                
                return false;
            }
            Card [] playerHand = RoundManager.instance.PlayerMove == Players.Player1 ? m_cardInHand1 : m_cardInHand2;
            int result = GetIndexLastCard(playerHand);
            playerHand[result] = moveCard;
            moveCard.StartCoroutine(moveCard.MoveInHandOrTable(moveCard, moveCard.m_curParent, CardState.InHand));
            return true;
        }

        public bool CardAttack(Card moveCard)
        {
            SlotScript slotScript = m_cardManger.GetClosestSlot(moveCard, false);
            AnimationComponent animationComponent = moveCard.GetComponent<AnimationComponent>();
            var slotTransform = slotScript.transform;

            if (slotScript != null && slotScript.couple && Vector3.Distance(moveCard.transform.position, slotTransform.position) < DragAndDropScript.MAGNET_RADIUS + 10f && slotScript.isActiveAndEnabled)
            {
                bool attackResult;
                if (slotScript.m_isPlyerAvatar)
                {
                    attackResult = slotScript.gameObject.GetComponent<PlayerScript>().GetDamage(moveCard.attack, false);
                    animationComponent.AnimationShakeCard();
                    moveCard.RefresMoveIndex(1);
                    if (attackResult)
                    {
                        Debug.LogError(RoundManager.instance.PlayerMove + " wins!");
                    }
                }
                else
                {
                     attackResult = slotScript.GetCardCouple().GetDamage(moveCard, true);
                }

                if (attackResult)
               {
                   slotScript.SwitchCouple(slotScript.GetCardCouple());
                   moveCard.RefresMoveIndex(1);
                   animationComponent.AnimationShakeCard();
               }

               moveCard.transform.position = new Vector3(slotTransform.position.x, slotTransform.position.y + 2f,
                    slotTransform.position.z);
                    
                moveCard.StartCoroutine(moveCard.MoveInHandOrTable(moveCard, moveCard.m_curParent, CardState.OnTable));
                
                return false;
            }
            moveCard.StartCoroutine(moveCard.MoveInHandOrTable(moveCard, moveCard.m_curParent, CardState.OnTable));
            
            return true;
        }

        private Card[] CheckTaunt()
        {
            Card[] tauntcards  = new Card[] {};

            return tauntcards;
        }
    }
    }
