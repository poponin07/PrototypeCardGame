
using System;
using System.Collections.Generic;
using Cards.ScriptableObjects;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Cards
{
    public class CardManager : MonoBehaviour
    {
        private List<CardPropertiesData> m_allCards;

        [SerializeField] private Card m_cardPrefab;
        [SerializeField] private CardPackConfiguration[] m_allPacks;
        [SerializeField, Range(0, 100)] private int m_cardDeckCount;

        [SerializeField, Space] private Transform m_player1DeckRoot;
        [SerializeField] private Transform m_player2DeckRoot;

        [SerializeField, Space] private PlayerHand _player1Hand;
        [SerializeField] private PlayerHand _player2Hand;

        private Card[] m_player1Deck;
        private Card[] m_player2Deck;

        private const float c_stepCardInDeck = 0.07f;
        private Material m_baseMat; 

        private Dictionary<int, SlotScript> m_playerCardSlots = new Dictionary<int, SlotScript>();
        [SerializeField] private PlayerCardSlotsManager m_slotsManager;
        private UIAvatarScript m_avatarScript;
        private PlayerData m_player1Data;
        private PlayerData m_player2Data;
            private void Awake()
        {
            CollectingAllCards();
        }

        private void Start()
        {
            Initialization();
        }

        private void Initialization()
        {
            m_player1Deck = CreateDeck(m_player1DeckRoot, Players.Player1);
            m_player2Deck = CreateDeck(m_player2DeckRoot, Players.Player2);
            m_avatarScript = GetComponent<UIAvatarScript>();
            m_player1Data = m_avatarScript._player1Data;
            m_player2Data = m_avatarScript._player2Data;
           
            FillCardSlots();
        }

        private void FillCardSlots()
        {
            foreach (var playerCardSlot in m_slotsManager.playerCardSlots)
            {
                m_playerCardSlots.Add(playerCardSlot.slotId, playerCardSlot);
            }
        }

        public SlotScript GetClosestSlot(Card card, bool isSamePlayerSlots)
        {
            float minDistance = float.MaxValue;
            var slotPositions = m_playerCardSlots;

            SlotScript closestSlot = slotPositions[0];
            foreach (var slot in slotPositions)
            {
                var slotPosition = slot.Value.transform.position;
                float distSlot = Vector3.Distance(card.transform.position, slotPosition);
                var player = RoundManager.instance.PlayerMove;
                bool isSamePlayer = player == Players.Player1 && slot.Value.playerId == 1 || player == Players.Player2 && slot.Value.playerId == 2;
                if (distSlot < minDistance && (isSamePlayerSlots == isSamePlayer))
                {
                    minDistance = distSlot;
                    closestSlot = slot.Value;
                }
            }

            return closestSlot;
        }
        
        private void CollectingAllCards()
        {
            IEnumerable<CardPropertiesData> _allCard = new List<CardPropertiesData>();
            foreach (var pack in m_allPacks)
            {
                _allCard = pack.UnionProperties(_allCard);
            }

            m_allCards = new List<CardPropertiesData>(_allCard);
            m_baseMat = new Material(Shader.Find("TextMeshPro/Sprite"));
            m_baseMat.renderQueue = 2994;
        }

        public void GetCardFromDeck()
        {
            int index;
            Card[] playerDeck;
            
            switch (RoundManager.instance.PlayerMove)
            {
                case Players.Player1:
                    playerDeck = m_player1Deck;
                    index = 0;
                    break;
                case Players.Player2:
                    playerDeck = m_player2Deck;
                    index = 0;
                    break;
                default:
                    playerDeck = m_player1Deck;
                    index = 0;
                    break;
            }
            
            for (int i = playerDeck.Length - 1; i >= 0; i--)
            {
                if (playerDeck[i] != null)
                {
                    index = i;
                    break;
                }
            }
            
            bool resultSetNewCard;
            
            switch (RoundManager.instance.PlayerMove)
            {
                case Players.Player1:
                    resultSetNewCard = _player1Hand.SetNewCardInHand(m_player1Deck[index]);
                    if (resultSetNewCard) m_player1Deck[index] = null;
                    break;
                case Players.Player2:
                    resultSetNewCard = _player2Hand.SetNewCardInHand(m_player2Deck[index]);
                    if (resultSetNewCard) m_player2Deck[index] = null;
                    break;
                default:
                    index = 0;
                    break;
            }
        }

        public bool CheckingCardRequirements(Card card)
        {
            PlayerData data = RoundManager.instance.PlayerMove == Players.Player1 ? m_player1Data : m_player2Data;
            if (card.coast <= data.Mana)
            {
                data.Mana -= card.coast;
                m_avatarScript.RefreshManaPlayer();
            }
            else
            {
                Debug.Log("Not enough mana");
                return false;
            }
            return true;
        }

        private Card[] CreateDeck(Transform root, Players player)
        {
            Card[] deck = new Card[m_cardDeckCount];
            Vector3 vector = Vector3.zero;

            for (int i = 0; i < m_cardDeckCount; i++)
            {
                deck[i] = Instantiate(m_cardPrefab, root);
                deck[i].cardManager = this;
                deck[i].transform.localPosition = vector;
                vector += new Vector3(0, c_stepCardInDeck, 0);

                var randomCard = m_allCards[Random.Range(0, m_allCards.Count)];

                var _newMat = new Material(m_baseMat);
                _newMat.mainTexture = randomCard.Texture;

                deck[i].Confiruration(randomCard, _newMat, CardUtility.GetDescriptionById(randomCard.Id), _player1Hand , _player2Hand, player);
            }

            return deck;
        }
    }
}