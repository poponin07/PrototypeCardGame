
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class PlayerHand : MonoBehaviour
    {
        [SerializeField] private Players m_move;
        [SerializeField] private Transform[] m_positonsCardInHand;
        //[SerializeField] private Transform[] m_positonsCardOnTable;
        //[SerializeField] private Card[] m_cardInHand; 
         private Card[] m_cardInHand1;
        private Card[] m_cardInHand2;
        private List<Card> m_cardOnTable;
        
        

        public Transform axis;
        private void Start()
        {
            if (m_move == Players.Player1)
                m_cardInHand1 = new Card[m_positonsCardInHand.Length];
            else
                m_cardInHand2 = new Card[m_positonsCardInHand.Length];
            
            m_cardOnTable = new List<Card>();
        }

        public bool SetNewCardInHand(Card newCard)
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
            DragAndDropScript dropScript = moveCard.GetComponent<DragAndDropScript>();
            Transform positonCardOnTable = dropScript.GetClosestSlot(dropScript.positonsCardOnTablePlayer1);
            
           switch (RoundManager.instance.PlayerMove)
            {
                case Players.Player1:
                    positonCardOnTable = dropScript.GetClosestSlot(dropScript.positonsCardOnTablePlayer1);
                    break;
                case Players.Player2:
                    positonCardOnTable = dropScript.GetClosestSlot(dropScript.positonsCardOnTablePlayer2);
                    break;
                case Players.Discard:
                    break;
            }
            
            Debug.DrawLine(moveCard.transform.position,positonCardOnTable.position, Color.red, 5);
            SlotScript slotScript = positonCardOnTable.GetComponent<SlotScript>();
            
            if ((slotScript == null ||  slotScript.couple) || Vector3.Distance(moveCard.transform.position, positonCardOnTable.position) > DragAndDropScript.MAGNET_RADIUS)
            {
                moveCard.StartCoroutine(moveCard.MoveInHand(moveCard, moveCard.m_curParent));
                return false;
            }

            slotScript.SwitchCouple();
            //m_cardOnTable.Add(moveCard);
            
                moveCard.m_curParent = positonCardOnTable;
                moveCard.transform.SetParent(positonCardOnTable);
                moveCard.transform.position = positonCardOnTable.position;
                moveCard.m_cardState = CardState.OnTable;
                //axis.GetComponent<RoundManager>().MoveChange();
                return true;
        }

        public bool CardAttack(Card moveCard)
        {
            DragAndDropScript dropScript = moveCard.GetComponent<DragAndDropScript>();
            Transform positonCardOnTable = dropScript.GetClosestSlot(dropScript.positonsCardOnTablePlayer1);
            Debug.Log(positonCardOnTable);
            
            switch (RoundManager.instance.PlayerMove)
            {
                case Players.Player1:
                    positonCardOnTable = dropScript.GetClosestSlot(dropScript.positonsCardOnTablePlayer2);
                    break;
                case Players.Player2:
                    positonCardOnTable = dropScript.GetClosestSlot(dropScript.positonsCardOnTablePlayer1);
                    break;
                case Players.Discard:
                    break;
            }
            Debug.DrawLine(moveCard.transform.position,positonCardOnTable.position, Color.red, 5);
            SlotScript slotScript = positonCardOnTable.GetComponent<SlotScript>();
            
            if ((slotScript == null ||  !slotScript.couple || Vector3.Distance(moveCard.transform.position, positonCardOnTable.position) > DragAndDropScript.MAGNET_RADIUS))
            {
                moveCard.StartCoroutine(moveCard.MoveInHand(moveCard, moveCard.m_curParent));
                return false;
            }

            return true;
        }
        
    }
    }
