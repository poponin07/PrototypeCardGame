using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cards
{
    public class DragAndDropScript : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler 
    {
        [SerializeField, Range(0,7)]public const float MAGNET_RADIUS = 3;
        [SerializeField] private PlayerHand m_player1Hand;
        [SerializeField] private PlayerHand m_player2Hand;
        private Card m_card;
        private Ray m_ray;
        public Transform[] positonsCardOnTablePlayer1;
        public Transform[] positonsCardOnTablePlayer2;
       

        private void Start()
        {
            m_card = GetComponent<Card>(); ;
            positonsCardOnTablePlayer1 = m_card.cardManager.MPositonsCardOnTablePlayer1;
            positonsCardOnTablePlayer2  = m_card.cardManager.MPositonsCardOnTablePlayer2;
            m_player1Hand = m_card.Player1Hand;
            m_player2Hand = m_card.Player2Hand;
        }

        
        public void OnBeginDrag(PointerEventData eventData)
        {
            switch (m_card.m_cardState)
            {
                case CardState.InDeck:
                    break;
                case CardState.InHand:
                    m_card.m_cardState = CardState.Discard;
                    m_card.transform.position = m_card.m_curParent.transform.position;
                    m_player1Hand.RemoveCardFromHand(m_card);
                    break;
                case CardState.OnTable:
                    break;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            
            switch (m_card.m_cardState)
            {
                case CardState.InDeck:
                    break;
                case CardState.InHand:
                   
                    break;
                case CardState.OnTable:
                    break;
                case CardState.Discard:
                    RaycastDragAndDrop(eventData);
                    break;
            }
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            switch (m_card.m_cardState)
            {
                case CardState.InDeck:
                break;
                case CardState.InHand:
                break;
                case CardState.OnTable:
                break;
                case CardState.Discard:
                    // Transform closestSlot = GetClosestSlot(positonsCardOnTablePlayer1);
                    // Debug.DrawLine(m_card.transform.position,closestSlot.position, Color.red, 5);
                    m_player1Hand.AddCardOnTable(m_card);
                    //MagnetToSlot(closestSlot);
                    break;
            }
        }

        private  void RaycastDragAndDrop(PointerEventData eventData)
        {
            Camera _camera = Camera.main;
            Vector3 cc = new Vector3(eventData.position.x,eventData.position.y,0);
            m_ray = _camera.ScreenPointToRay(cc);
                    
                    
            if (Physics.Raycast(m_ray, out var hit))
            {
                Debug.DrawLine(m_ray.origin, hit .point, Color.green);
                m_card.transform.position = new Vector3(hit.point.x, 0, hit.point.z);
            } 
        }

        public Transform GetClosestSlot(Transform[] slotPositions)
        {
            float minDistance = float.MaxValue;
            Transform closestSlot = slotPositions[0];
            foreach (var slot in slotPositions)
            {
                float distSlot = Vector3.Distance(m_card.transform.position, slot.position);

                if (distSlot < minDistance)
                {
                    minDistance = distSlot;
                    closestSlot = slot;
                }
            }
            return closestSlot;
        }

        /*public void MagnetToSlot(Transform slot)
        {
            if (Vector3.Distance(m_card.transform.position, slot.position) <= m_magnetRadius)
            {
                m_card.m_curParent = slot;
                m_card.transform.SetParent(slot);
                m_card.transform.position = slot.position;
                m_card.m_cardState = CardState.OnTable;
            }
            else
            {
                StartCoroutine(m_card.MoveInHand(m_card, m_card.m_curParent));
            }
        }*/
        
    }
}