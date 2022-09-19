using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cards
{
    public class DragAndDropScript : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler 
    {
        [SerializeField, Range(0,7)]private float m_magnetRadius = 7;
        private Card m_card;
        private Ray m_ray;
        public Transform[] positonsCardOnTablePlayer1;
        public Transform[] positonsCardOnTablePlayer2;
       

        private void Start()
        {
            m_card = GetComponent<Card>(); ;
            positonsCardOnTablePlayer1 = m_card.cardManager.MPositonsCardOnTablePlayer1;
            positonsCardOnTablePlayer2  = m_card.cardManager.MPositonsCardOnTablePlayer2;
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
                    
                    RaycastDragAndDop(eventData);
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
                    
                    Transform closestSlot = GetClosestSlot(positonsCardOnTablePlayer1);
                    Debug.DrawLine(m_card.transform.position,closestSlot.position, Color.red, 5);
                    MagnetToSlot(closestSlot);
                    break;
            }
        }

        private  void RaycastDragAndDop(PointerEventData eventData)
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

        private Transform GetClosestSlot(Transform[] slotPositions)
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

        private void MagnetToSlot(Transform slot)
        {
            if (Vector3.Distance(m_card.transform.position, slot.position) <= m_magnetRadius)
            {
                m_card.m_curParent = slot;
                m_card.transform.position = slot.position;
            }
            else
            {
                StartCoroutine(m_card.MoveInHand(m_card, m_card.m_curParent));
            }
        }




    }
}