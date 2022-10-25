using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Cards
{
    public class PlayerHand : MonoBehaviour
    {
        [SerializeField] private Transform[] m_positonsCardInHand;
        //[SerializeField] private Transform[] m_positonsCardOnTable;
        private Card[] m_cardInHand;
        private List<Card> m_cardOnTable;

        public Transform axis;
        private void Start()
        {
            m_cardInHand = new Card[m_positonsCardInHand.Length];
            m_cardOnTable = new List<Card>();
        }

        public bool SetNewCardInHand(Card newCard)
        {
            int result = GetIndexLastCard(m_positonsCardInHand);

            if (result == -1)
            {
                Debug.LogWarning("Maximum number of cards in a hand");
                return false;
            }

            m_cardInHand[result] = newCard;

            newCard.StartCoroutine(newCard.LiftCard(newCard, m_positonsCardInHand[result]));

            return true;
        }

        private int GetIndexLastCard(Transform[] arr)
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

        public void RemoveCardFromHand(Card removeCard)
        {
            for (int i = 0; i < m_cardInHand.Length - 1; i++)
            {
                if (m_cardInHand[i] == removeCard)
                {
                    m_cardInHand[i] = null;
                    Debug.Log("Еhe card is removed from the hand");
                    return;
                }
            } 
        }
        
        public bool AddCardOnTable(Card moveCard)
        {
            DragAndDropScript dropScript = moveCard.GetComponent<DragAndDropScript>();
            Transform positonCardOnTable = dropScript.GetClosestSlot(dropScript.positonsCardOnTablePlayer1);
            Debug.DrawLine(moveCard.transform.position,positonCardOnTable.position, Color.red, 5);
            SlotScript slotScript = positonCardOnTable.GetComponent<SlotScript>();
            
            if ((slotScript == null ||  slotScript.couple) || Vector3.Distance(moveCard.transform.position, positonCardOnTable.position) > DragAndDropScript.MAGNET_RADIUS)
            {
                moveCard.StartCoroutine(moveCard.MoveInHand(moveCard, moveCard.m_curParent));
                return false;
            }

            slotScript.SwitchCouple();
            m_cardOnTable.Add(moveCard);
            
                moveCard.m_curParent = positonCardOnTable;
                moveCard.transform.SetParent(positonCardOnTable);
                moveCard.transform.position = positonCardOnTable.position;
                moveCard.m_cardState = CardState.OnTable;
                (axis.GetComponent<RoundManager>()).MoveChange();
                return true;
        }
    }
    }
