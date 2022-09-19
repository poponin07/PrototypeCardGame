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
        [SerializeField, Space] private GameObject _frontCard;
        [SerializeField] private TextMeshPro m_coast;
        [SerializeField] private TextMeshPro m_attack;
        [SerializeField] private TextMeshPro m_health;
        [SerializeField] private TextMeshPro m_name;
        [SerializeField] private TextMeshPro m_descriptions;
        [SerializeField] private TextMeshPro m_cardUnitType;
        [SerializeField, Space] private uint m_id;
        [SerializeField] private MeshRenderer _picture;
        public AnimationComponent animationComponent;
        private PlayerHand m_playerHand;
        private float stepY = 10f;
        private float liftSpeed = 0.05f;
        private float liftHeight = 5f;
        
        public Transform m_curParent;

        public CardManager cardManager;
        
        public bool isFrontSide => _frontCard.activeSelf;


        public  CardState m_cardState;

        public void Confiruration(CardPropertiesData data, Material picture, string description, PlayerHand playerHand)
        {
            m_playerHand = playerHand;
            m_coast.text = data.Cost.ToString();
            m_attack.text = data.Attack.ToString();
            m_health.text = data.Health.ToString();
            m_descriptions.text = description;
            m_name.text = data.Name;

            m_cardUnitType.text = CardUnitType.None == data.Type ? "" : data.Type.ToString();
            _picture.material = picture;

            m_id = data.Id;
            animationComponent.Link(m_playerHand);
        }

        [ContextMenu("SwitchEnable")]
        public void SwitchEnable()
        {
            bool turneBool = !isFrontSide;
            _frontCard.SetActive(turneBool);
            _picture.enabled = turneBool;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            switch (m_cardState)
            {
                case CardState.InDeck:

                    break;
                case CardState.InHand:
                    transform.localPosition = new Vector3(0, stepY, 0);
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

        public IEnumerator MoveInHand(Card card, Transform parent)
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
            card.m_cardState = CardState.InHand;
            //card.SwitchEnable();
        }
    }
} 