using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Cards
{
    public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField, Space] private GameObject m_frontCard;
        [SerializeField] private TextMeshPro m_tx_coast;
        [SerializeField] private TextMeshPro m_tx_attack;
        [SerializeField] private TextMeshPro m_tx_health;
        [SerializeField] private TextMeshPro m_name;
        [SerializeField] private TextMeshPro m_tx_descriptions;
        [SerializeField] private TextMeshPro m_tx_cardUnitType;
        
        [SerializeField, Space] private uint m_id;
        [SerializeField] private MeshRenderer _picture;

        public int m_health;
        public int m_coast;
        public int m_attack;
        
        public Players players;
        public AnimationComponent animationComponent;
        private PlayerHand m_player1Hand;
        private PlayerHand m_player2Hand;
        private float stepY = 10f;
        private float liftSpeed = 0.05f;
        private float liftHeight = 5f;
        private bool isMove;
        
        public Transform m_curParent;

        public CardManager cardManager;
        
        public bool isFrontSide => m_frontCard.activeSelf;

        public PlayerHand Player1Hand => m_player1Hand;

        public PlayerHand Player2Hand => m_player2Hand;


        public  CardState m_cardState;

        public void Confiruration(CardPropertiesData data, Material picture, string description, PlayerHand playerHand1, PlayerHand playerHand2, Players player)
        {
            m_player1Hand = playerHand1;
            m_player2Hand = playerHand2;
            m_tx_coast.text = data.Cost.ToString();
            m_tx_attack.text = data.Attack.ToString();
            m_tx_health.text = data.Health.ToString();
            m_tx_descriptions.text = description;
            m_name.text = data.Name;
            players = player;
            m_health = data.Health;
            m_attack = data.Attack;
            m_coast = data.Cost;
            m_tx_cardUnitType.text = CardUnitType.None == data.Type ? "" : data.Type.ToString();
            _picture.material = picture;
           m_cardState = CardState.InDeck;
            SwitchEnable();
            m_id = data.Id;
            animationComponent.Link(Player1Hand);
            isMove = false;
        }

        private void RefreshUICard()
        {
            m_tx_coast.text = m_coast.ToString();
            m_tx_attack.text = m_attack.ToString();
            m_tx_health.text = m_health.ToString();
        }

        [ContextMenu("SwitchEnable")]
        public void SwitchEnable()
        {
            bool turneBool = !isFrontSide;
            m_frontCard.SetActive(turneBool);
            _picture.enabled = turneBool;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            switch (m_cardState)
            {
                case CardState.InDeck:

                    break;
                case CardState.InHand:
                    if (players == RoundManager.instance.PlayerMove)
                    {
                        transform.localPosition = new Vector3(0, stepY, 0);
                    }
                    break;
                case CardState.OnTable:
                    break;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            switch (m_cardState)
            {
                case CardState.InDeck:

                    break;
                case CardState.InHand:
                    transform.localPosition = new Vector3(0, 0, 0);
                    break;
                case CardState.OnTable:
                    break;
            }
        }

        public IEnumerator LiftCard(Card card, Transform parent)
        {
            Vector3 startPos = card.transform.position;
            Vector3 endPos = card.transform.position + (Vector3.up * liftHeight);
            m_curParent = parent;

            while (Vector3.Distance(endPos, startPos) > 0.01f)
            {
                startPos = card.transform.position;
                yield return null;
                card.transform.Translate((endPos - startPos) * (Time.deltaTime + liftSpeed));
            }
            card.animationComponent.AnimationFlipCard();
        }

        public IEnumerator MoveInHandOrTable(Card card, Transform parent, CardState cardState)
        {
            Vector3 startPos = card.transform.position;
            Vector3 endPos = parent.transform.position;
            float time = 0f;
            while (time < 1f)
            {
                card.transform.SetParent(parent);
                card.transform.position = Vector3.Lerp(startPos, endPos, time);
                time += Time.deltaTime;
                yield return null;
            }
            card.m_cardState = cardState;
        }


        public bool GetDamage(int damage)
        {
            m_health -= damage;

            if (m_health <= 0)
            {
                DestroyCard();
                return true;
            }

            RefreshUICard();
            return false;
        }
        
        private void DealDamage()
        {
            
        }

        private void DestroyCard()
        {
            Destroy(gameObject);
        }
    }
} 