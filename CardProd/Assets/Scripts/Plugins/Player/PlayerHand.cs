
using System;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Cards
{
    public class PlayerHand : MonoBehaviour
    {
        //[SerializeField] private Players m_move;
        [SerializeField] private Transform[] m_positonsCardInHand; //позиции для карт в руке

        [SerializeField] private CardManager m_cardManger;
        
        public Card[] m_cardInHand1; //карты в руке игрок 1
        public Card[] m_cardInHand2; //карты в руке игрок 2

       // public Transform axis;
        private void Start()
        {
            m_cardInHand1 = new Card[m_positonsCardInHand.Length];
            m_cardInHand2 = new Card[m_positonsCardInHand.Length];
        }
        
        //получение карты в уку
        public bool SetNewCardInHand(Card newCard, bool fromDeck)
        {
            Card[] cardInHand;
            if (RoundManager.instance.PlayerMove == Players.Player1)
            {
                cardInHand = m_cardInHand1;
            }
            else
            {
                cardInHand = m_cardInHand2;
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

        //последняя карта
        public int GetIndexLastCard(Card[] arr)
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

        //убирает карту из руки
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
        
        //добавляет карту на стол
        public bool AddCardOnTable(Card moveCard, CardState cardState)
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
                
                m_cardManger.SetEffectOnCard(moveCard);
                moveCard.SwitchCardState(moveCard,CardState.OnTable);
                return true;
            }
            Card [] playerHand = RoundManager.instance.PlayerMove == Players.Player1 ? m_cardInHand1 : m_cardInHand2;
            int result = GetIndexLastCard(playerHand);
            playerHand[result] = moveCard;
            moveCard.StartCoroutine(moveCard.MoveInHandOrTable(moveCard, moveCard.m_curParent, CardState.InHand));
            return false;
        }

        //добавляет призванную карту на стол
        public void AddSummoncardOnTable(Card moveCard, CardState cardState)
        {
            SlotScript slot = m_cardManger.GetFreeSlotOnTable(RoundManager.instance.PlayerMove);
            var slotTransform = slot.transform;

            if (slot != null)
            {
                slot.SwitchCouple(moveCard);
                moveCard.m_curParent = slotTransform;
                moveCard.transform.SetParent(slotTransform);

                List<Card> arr = moveCard.players  == Players.Player1 ? m_cardManger.cardsPlayedPlayer1 : m_cardManger.cardsPlayedPlayer2;
                arr.Add(moveCard);
                
                //m_cardManger.SetEffectOnCard(moveCard);
                
               
                Card [] playerHand = RoundManager.instance.PlayerMove == Players.Player1 ? m_cardInHand1 : m_cardInHand2;
                int result = GetIndexLastCard(playerHand);
                //playerHand[result] = moveCard;
                moveCard.StartCoroutine(moveCard.MoveInHandOrTable(moveCard, moveCard.m_curParent, cardState));
                moveCard.SwitchCardState(moveCard,CardState.OnTable);
            }

        }
        
        //атака карты
        public bool CardAttack(Card moveCard)
        {
            SlotScript slotScript = m_cardManger.GetClosestSlot(moveCard, false);
            AnimationComponent animationComponent = moveCard.GetComponent<AnimationComponent>();
            var slotTransform = slotScript.transform;
            List<Card> TauntCards = CheckTaunt();

            if ( slotScript != null && slotScript.couple && 
                Vector3.Distance(moveCard.transform.position, slotTransform.position) < DragAndDropScript.MAGNET_RADIUS + 10f && slotScript.isActiveAndEnabled)
            {
                bool attackResult = false;
           
                if (slotScript.m_isPlyerAvatar)
                {
                    slotScript.gameObject.GetComponent<PlayerScript>().GetDamage(moveCard.Attack, false);
                    animationComponent.AnimationShakeCard();
                    moveCard.RefresMoveIndex(1);
                }
                else
                {
                    if (TauntCards.Count > 0 && TauntCards.Contains(slotTransform.GetComponentInChildren<Card>()))
                    {
                        attackResult = slotScript.GetCardCouple().GetDamage(moveCard, true); 
                    }

                    if (TauntCards.Count == 0)
                    {
                        attackResult = slotScript.GetCardCouple().GetDamage(moveCard, true);  
                    }
                }

                if (attackResult)
               {
                   //slotScript.SwitchCouple(slotScript.GetCardCouple());
                   moveCard.RefresMoveIndex(1);
                   animationComponent.AnimationShakeCard();
               }

                var position = slotTransform.position;
                moveCard.transform.position = new Vector3(position.x, position.y + 2f,
                    position.z);
                    
                moveCard.StartCoroutine(moveCard.MoveInHandOrTable(moveCard, moveCard.m_curParent, CardState.OnTable));
                
                return false;
            }
            moveCard.StartCoroutine(moveCard.MoveInHandOrTable(moveCard, moveCard.m_curParent, CardState.OnTable));
            
            return true;
        }

        //првоерка на таунт
        private List<Card> CheckTaunt()
        {
            List<Card> tauntCards = new List<Card>();
                List<Card> cards = RoundManager.instance.PlayerMove == Players.Player1
                ? m_cardManger.cardsPlayedPlayer2
                : m_cardManger.cardsPlayedPlayer1;
            
            foreach (var card in cards)
            {
                if (card.isTaunt)
                {
                    tauntCards.Add(card);
                }
            }
            
            return tauntCards;
        }
    }
    }
