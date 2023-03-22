using System;
using System.Collections;
using System.Collections.Generic;
using examples;
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

        public int health;
        public int coast;
        public int attack;
        
        public CardManager cardManager;
        public Transform m_curParent;
        public Players players;
        public AnimationComponent animationComponent;
        public int isReadyMome;
        public bool isTaunt;
        private PlayerHand m_player1Hand;
        private PlayerHand m_player2Hand;
        private float stepY = 10f;
        private float liftSpeed = 0.05f;
        private float liftHeight = 5f;
        private SlotInhandScript m_slotInhandScript;

        private Transform m_deckPosition;

        
        
        public bool isFrontSide => m_frontCard.activeSelf;

        public PlayerHand Player1Hand => m_player1Hand;

        public PlayerHand Player2Hand => m_player2Hand;


        public  CardState m_cardState;

        public void Confiruration(CardPropertiesData data, Material picture, string description, PlayerHand playerHand1, PlayerHand playerHand2, Players player)
        {
            List<BaseEffect> effects = new List<BaseEffect>();
            foreach (var dataEffect in data.effects)
            {
                // effects
            }
            m_player1Hand = playerHand1;
            m_player2Hand = playerHand2;
            m_tx_coast.text = data.Cost.ToString();
            m_tx_attack.text = data.Attack.ToString();
            m_tx_health.text = data.Health.ToString();
            m_tx_descriptions.text = description;
            m_name.text = data.Name;
            players = player;
            health = data.Health;
            attack = data.Attack;
            coast = data.Cost;
            isTaunt = data.isTaunt;
            m_tx_cardUnitType.text = CardUnitType.None == data.Type ? "" : data.Type.ToString();
            _picture.material = picture;
            SwitchCardState(this, CardState.InDeck);
            SwitchEnable();
            m_id = data.Id;
            animationComponent.Link(Player1Hand);
            SetChargeParam(data);
            m_deckPosition = RoundManager.instance.PlayerMove == players
                ? cardManager.m_player1DeckRoot
                : cardManager.m_player2DeckRoot;
        }

        private void SetChargeParam(CardPropertiesData data)
        {
            if (data.isCharge == true)
            {
                isReadyMome = 0;
            }
            else
            {
                isReadyMome = 1;
            }
        }
        
        private void RefreshUICard()
        {
            m_tx_coast.text = coast.ToString();
            m_tx_attack.text = attack.ToString();
            m_tx_health.text = health.ToString();
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

        public IEnumerator LiftCard(Card card, Transform parent, bool fromDeck)
        {
            Vector3 startPos = card.transform.position;
            Vector3 endPos = card.transform.position + (Vector3.up * liftHeight);
            m_curParent = parent;

            if (fromDeck)
            {
                while (Vector3.Distance(endPos, startPos) > 0.01f)
                {
                    startPos = card.transform.position;
                    yield return null;
                    card.transform.Translate((endPos - startPos) * (Time.deltaTime + liftSpeed));
                }
            }

            card.animationComponent.AnimationFlipCard();
        }

        public IEnumerator MoveInHandOrTable(Card card, Transform parent, CardState cardState)
        {
            Vector3 startPos = card.transform.position;
            Vector3 endPos = parent.transform.position;
            float time = 0f;
            card.transform.SetParent(parent);

            while (time < 1f)
            {
                
                card.transform.position = Vector3.Lerp(startPos, endPos, time);
                time += Time.deltaTime;
                yield return null;
            }
            SwitchCardState(card, cardState);
        }

        public void SwitchCardState(Card card, CardState cardState)
        {
            card.m_cardState = cardState;
        }

        public void RefresMoveIndex(int arg)
        {
            isReadyMome += arg;
        }
        
        public bool GetDamage(Card attackingCard, bool  firstAttack)
        {
            health -= attackingCard.attack;
            animationComponent.AnimationScaleCard();
            
            if (firstAttack)
            {
                attackingCard.GetDamage(this, false);
            }

            if (health <= 0)
            {
                m_curParent.GetComponent<SlotScript>().SwitchCouple(null);
                DestroyCard();
                return true;
            }

            RefreshUICard();
            
            return false;
        }

        public SlotInhandScript GetSlotInCurHand()
        {
            return m_slotInhandScript;
        }

        public bool CardInDeck()
        {
            if (m_slotInhandScript.GetIsSwapedCardOnFerstRound())
            {
                StartCoroutine(LiftCard( this, m_deckPosition, false));
                m_slotInhandScript.SetIsSwapedCardOnFerstRound();
                return true;
            }

            return false;
        }
        
        public void SetSlotInCurHand(SlotInhandScript slotInhandScript)
        {
            m_slotInhandScript = slotInhandScript;
        }
        
        private void DestroyCard()
        {
            List<Card> arr = RoundManager.instance.PlayerMove == players ? cardManager.cardsPlayedPlayer1 : cardManager.cardsPlayedPlayer2;
            arr.Remove(this);
            Destroy(gameObject);
        }
        
        
        //effect
        private int m_defaultHealth = 1;
        [SerializeField] private int m_defaultDamage = 0;
        
        private LinkedList<BaseEffect> m_effects = new LinkedList<BaseEffect>();
        
        public void AddEffect(BaseEffect effect)
        {
            m_effects.AddLast(effect);
            effect.ApplyEffect(this);
        }

        public bool TryToRemoveEffect(BaseEffect effect)
        {
            if (!m_effects.Contains(effect)) return false;
            m_effects.Remove(effect);

            return effect.TryToRemoveEffect(this);
        }

        private void Awake()
        {
            health = m_defaultHealth;
            attack = m_defaultDamage;
        }

    }
} 