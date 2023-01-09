using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cards
{
    public class DragAndDropScript : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField, Range(0, 7)] public const float MAGNET_RADIUS = 3;
       private PlayerHand m_player1Hand;
        [SerializeField] private PlayerHand m_player2Hand;
        private float m_magnetRadius = 7;
        private Card m_card;
        private Ray m_ray;
        private void Start()
        {
            m_card = GetComponent<Card>();
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
                    var player = RoundManager.instance.PlayerMove;
                    if (player != m_card.players)
                    {
                        return;
                    }

                    if (m_card.m_cardState == CardState.InHand)
                    {
                        m_card.SwitchCardState(m_card, CardState.Discard);
                    }

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
                    //RaycastDragAndDrop(eventData);
                    break;
                case CardState.OnTable:
                    var player = RoundManager.instance.PlayerMove;
                    if (player == m_card.players && m_card.isReadyMome <= 0)
                    {
                        RaycastDragAndDrop(eventData);
                    }
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
                    //m_player1Hand.SetNewCardInHand(m_card, false);
                   
                    break;
                case CardState.OnTable:
                    var player = RoundManager.instance.PlayerMove;
                    if (player == m_card.players)
                    {
                        switch (RoundManager.instance.PlayerMove)
                        {
                            case Players.Player1:
                                m_player2Hand.CardAttack(m_card);
                                break;
                            case Players.Player2:
                                m_player1Hand.CardAttack(m_card);
                                break;
                            case Players.Discard:
                                Debug.LogError("Discard in Drop");
                                break;
                        }
                    }
                    break;
                case CardState.Discard:
                    
                    PlayerHand playerHand = RoundManager.instance.PlayerMove == Players.Player1 ? m_player1Hand : m_player2Hand;
                    m_player1Hand.AddCardOnTable(m_card);
                    //m_card.StartCoroutine(m_card.MoveInHandOrTable(m_card,m_card.m_curParent, CardState.InHand));
                    //m_card.StartCoroutine(m_card.MoveInHandOrTable(m_card));
                    
                    break;
            }
        }

        private void RaycastDragAndDrop(PointerEventData eventData)
        {
            Camera _camera = Camera.main;
            Vector3 dirVector = new Vector3(eventData.position.x, eventData.position.y + 2f, 0);
            m_ray = _camera.ScreenPointToRay(dirVector);


            if (Physics.Raycast(m_ray, out var hit))
            {
                Debug.DrawLine(m_ray.origin, hit.point, Color.green);
                m_card.transform.position = new Vector3(hit.point.x, 1, hit.point.z);
            }

            /*if (m_card.m_cardState != CardState.Discard)
            {
                m_card.SwitchCardState(m_card, CardState.Discard);
            }*/
        }
    }
}