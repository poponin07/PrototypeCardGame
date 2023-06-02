using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using effects;
using JetBrains.Annotations;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Cards
{
    public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public int cardIndex;
        [SerializeField, Space] private GameObject m_frontCard;
        [SerializeField] private TextMeshPro m_tx_coast;
        [SerializeField] private TextMeshPro m_tx_attack;
        [SerializeField] private TextMeshPro m_tx_health;
        [SerializeField] private TextMeshPro m_name;
        [SerializeField] private TextMeshPro m_tx_descriptions;
        [SerializeField] private TextMeshPro m_tx_cardUnitType;
        
        [SerializeField, Space] private uint m_id;
        [SerializeField] private MeshRenderer _picture;

        private int m_health; //здоровье
        private int m_coast; //стоимость использования
        private int m_attack; //атака
        public int healthDefaulte; //дефолтное здоровье карты
        public int attackDefaulte; //дефолтная  атака карты

        public int Health
        {
            get
            {
                return m_health;
            }
            set
            {
                m_health = value;
                UpdateUICard();
            }
        } // свойство здороья 
        public int Coast
        {
            get
            {
                return m_coast;
            }
            set
            {
                m_coast = value;
                UpdateUICard();
            }
        } //свойство цены
        public int Attack {
            get
            {
                return m_attack;
            }
            set
            {
                m_attack = value;
                UpdateUICard();
            } } // свойство атаки
        
        public CardManager cardManager;
        public Transform m_curParent; //текущий родитель
        public Players players; //какому игроку принадлежит
        public AnimationComponent animationComponent;
        public int isReadyMome; //флаг возможности хода в текущем раунде
        public bool isTaunt; //флаг для карт с наунтом
        public bool isSummon; //флаг для призванных карт
        public bool effectWhenTakingDamage; //флаг для карт со свойством при получении урона
        private PlayerHand m_player1Hand;
        private PlayerHand m_player2Hand;
        private float stepY = 10f;
        private float liftSpeed = 0.05f;
        private float liftHeight = 5f;
        private int summonID;
        private SlotInhandScript m_slotInhandScript;
        private CardPropertiesData m_cardData;
        private Transform m_deckPosition;
        private BaseEffect effect;
        

        
        
        public bool isFrontSide => m_frontCard.activeSelf;

        public PlayerHand Player1Hand => m_player1Hand;

        public PlayerHand Player2Hand => m_player2Hand;


        public  CardState m_cardState;

        //конфигурация карты
        public void Confiruration(CardPropertiesData data, Material picture, string description, PlayerHand playerHand1, PlayerHand playerHand2, Players player)
        {
            cardIndex = Random.Range(0, 999);
            effect = data.effect;
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
            healthDefaulte = m_health;
            attackDefaulte = m_attack;
            m_coast = data.Cost;
            isTaunt = data.isTaunt;
            isSummon = data.iSummon;
            summonID = data.summonIDCard;
            healthDefaulte = m_health;
            attackDefaulte = m_attack;
            effectWhenTakingDamage = false;
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
            m_cardData = data;
        }
        
        //переключатель флага хода для карты
        private void SetChargeParam(CardPropertiesData data)
        {
            if (data.isCharge)
            {
                isReadyMome = 0;
            }
            else
            {
                isReadyMome = 1;
            }
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

        //карта поднимается 
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

        //перемещение карты в руку или на стол
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

        //смена состояния (на столе, в руке)
        public void SwitchCardState(Card card, CardState cardState)
        {
            card.m_cardState = cardState;
        }

        //создал индекс для управления ходами
        public void RefresMoveIndex(int arg)
        {
            isReadyMome += arg;
        }
        
        //карта получает урон
        public bool GetDamage(Card attackingCard, bool  firstAttack)
        {
            m_health -= attackingCard.m_attack;
            animationComponent.AnimationScaleCard();

            if (effectWhenTakingDamage)
            {
                cardManager.SetEffectOnCard(this);  
            }
           
            
            if (firstAttack)
            {
                attackingCard.GetDamage(this, false);
            }

            if (m_health <= 0)
            {
                m_curParent.GetComponent<SlotScript>().SwitchCouple(null);
                Card[] hand = new Card[] {};
                if (RoundManager.instance.PlayerMove == Players.Player1)
                {
                    hand = cardManager._player2Hand.m_cardInHand2;
                }
                else
                {
                    hand = cardManager._player1Hand.m_cardInHand1;
                }
                DestroyCard(hand);
                return true;
            }
            
            UpdateUICard();
            
            return false;
        }

        //урон от способности
        public void GetDamageSpell(int damage)
        {
            m_health = -damage;

            if (m_health <= 0)
            {
                m_curParent.GetComponent<SlotScript>().SwitchCouple(null);
                
                Card[] hand = new Card[] {};
                if (RoundManager.instance.PlayerMove == Players.Player1)
                {
                    hand = cardManager._player2Hand.m_cardInHand2;
                }
                else
                {
                    hand = cardManager._player1Hand.m_cardInHand1;
                }
                
                DestroyCard(hand);
            }
        }

        /*public SlotInhandScript GetSlotInCurHand()
        {
            return m_slotInhandScript;
        }*/

        //возвращение карты колоду
        public bool CardInDeck(Card card)
        {
            if (!m_slotInhandScript.GetIsSwapedCardOnFerstRound())
            {
                PlayerHand playerHand =
                    RoundManager.instance.PlayerMove == Players.Player1 ? m_player1Hand : m_player2Hand;
                playerHand.RemoveCardFromHand(card);
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
        
        //уничтожение карты
        public void DestroyCard(Card[] hand)
        {
            if (effect != null)
            {
                effect.TryToRemoveEffect(cardManager);
            }

            List<Card> arr = RoundManager.instance.PlayerMove == players ? cardManager.cardsPlayedPlayer1 : cardManager.cardsPlayedPlayer2;
            arr.Remove(this);
            if (isSummon)
            {
                if (players == Players.Player1)
                {
                    cardManager.SummonedCardPlayer1.Remove(this);
                }
                else
                {
                    cardManager.SummonedCardPlayer2.Remove(this);
                }
            }

            for (int i = 0; i < hand.Length - 1; i++)
            {
                if (hand[i] == this)
                {
                    hand[i] = null;
                    break;
                }
            }
            
            SlotScript slotScript = m_curParent.transform.GetComponent<SlotScript>();
            if (slotScript)
            {
                Card card = slotScript.GetCardCouple();
                if (card == this)
                {
                    slotScript.couple = false;
                }
            }

            Destroy(gameObject);
        }
        
        //возвращает тип карты
        public CardUnitType GetCardType()
        {
            return m_cardData.Type;
        }
        

       // private int m_defaultHealth = 1;
        //[SerializeField] private int m_defaultDamage = 0;
        

        public CardPropertiesData GetDataCard()
        {
            return m_cardData;
        }

        //обновляет UI карты
        private void UpdateUICard()
        {
            m_tx_health.text = m_health.ToString();
            m_tx_attack.text = m_attack.ToString();

        }
    }
} 