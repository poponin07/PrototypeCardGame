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
        private IEnumerable<CardPropertiesData> allCard;
        [SerializeField, Range(0, 100)] private int m_cardDeckCount;

        public Transform m_player1DeckRoot;
        public Transform m_player2DeckRoot;

        [SerializeField, Space] private PlayerHand _player1Hand;
        [SerializeField] private PlayerHand _player2Hand;

        [SerializeField] private PlayerScript m_playerScript1;
        [SerializeField] private PlayerScript m_playerScript2;

        public List<Card> cardsPlayedPlayer1;
        public List<Card> cardsPlayedPlayer2;

        private Card[] m_player1Deck;
        private Card[] m_player2Deck;
        private Card[] m_customPlayer1Deck;

        private Card[] m_customPlayer2Deck;

        // private Dictionary<Card, Card> actualEffects = new Dictionary<Card, Card>();
        [SerializeField] private int[] m_idForCustomDeckPlayer1;
        [SerializeField] private int[] m_idForCustomDeckPlayer2;


        private const float c_stepCardInDeck = 0.07f;
        private Material m_baseMat;

        private Dictionary<int, SlotScript> m_playerCardSlots = new Dictionary<int, SlotScript>();

        [SerializeField] private PlayerCardSlotsManager m_slotsManager;
        public List<SlotScript> slotsOntablePlayer1;
        public List<SlotScript> slotsOntablePlayer2;
        private UIAvatarScript m_avatarScript;
        public PlayerData m_player1Data;
        public PlayerData m_player2Data;
        public bool UseCustomDeck;

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

            foreach (var cardSlot in m_playerCardSlots)
            {
                if (!cardSlot.Value.m_isPlyerAvatar)
                {
                    if (cardSlot.Value.playerId == 1)
                    {
                        slotsOntablePlayer1.Add(cardSlot.Value);
                    }
                    else
                    {
                        slotsOntablePlayer2.Add(cardSlot.Value);
                    }
                }
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
                bool isSamePlayer = player == Players.Player1 && slot.Value.playerId == 1 ||
                                    player == Players.Player2 && slot.Value.playerId == 2;
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
            allCard = new List<CardPropertiesData>();
            foreach (var pack in m_allPacks)
            {
                allCard = pack.UnionProperties(allCard);
            }

            m_allCards = new List<CardPropertiesData>(allCard);
            m_baseMat = new Material(Shader.Find("TextMeshPro/Sprite"));
            m_baseMat.renderQueue = 2994;
        }

        public void GetCardFromDeck(int cards, bool isRandomCard)
        {
            PlayerHand playerHand = RoundManager.instance.PlayerMove == Players.Player1 ? _player1Hand : _player2Hand;

            for (int j = cards; j != 0; j--)
            {
                Card card = null;
                Card[] playerDeck;

                playerDeck = RoundManager.instance.PlayerMove == Players.Player1 ? m_player1Deck : m_player2Deck;

                int randomCardIndex = 0;
                if (isRandomCard)
                {
                    var result = GetRandomIndexFromDeck(playerDeck);
                    card = result.card;
                    randomCardIndex = result.index;
                }

                if (!isRandomCard)
                {
                    for (int i = playerDeck.Length - 1; i >= 0; i--)
                    {
                        if (playerDeck[i] != null)
                        {
                            card = playerDeck[i];
                            randomCardIndex = i;
                            break;
                        }
                    }
                }

                if (!CheckDeckNull(playerDeck))
                {
                    PlayerScript playerScript = RoundManager.instance.PlayerMove == Players.Player1
                        ? m_playerScript1
                        : m_playerScript2;
                    playerScript.GetDamage(playerScript.GetDamageForEmptyDeck(), true);
                }

                if (card != null && CheckDeckNull(playerDeck) && playerHand.SetNewCardInHand(card, true))
                {
                    playerDeck[randomCardIndex] = null;
                }
            }
        }

        private SlotScript GetFreeSlotOnTable(Players player)
        {
            List<SlotScript> slots = new List<SlotScript>();
            if (player == Players.Player1)
            {
                slots = slotsOntablePlayer1;
            }
            else
            {
                slots = slotsOntablePlayer2;
            }

            foreach (var slot in slots)
            {
                if (slot.couple == false)
                {
                    return slot;
                }
            }

            return null;
        }

        public void AddSummonCardOntable(int id)
        {
            
            SlotScript freeSlot = GetFreeSlotOnTable(RoundManager.instance.PlayerMove);
            if (GetFreeSlotOnTable(RoundManager.instance.PlayerMove) == true)
            {
                /*List<SlotScript> slotsOnTable = RoundManager.instance.PlayerMove == Players.Player1
                    ? slotsOntablePlayer1
                    : slotsOntablePlayer2;
                SlotScript freeSlot = null;
                foreach (var slot in slotsOnTable)
                {
                    if (slot.couple == false)
                    {
                        freeSlot = slot;
                    }
                }

                if (freeSlot == null)
                {
                    return;

                }
*/
                CardPropertiesData cardDataForSpaun = m_allCards[0];
                bool res = false;
                foreach (var cardData in allCard)
                {
                    if (cardData.Id == id)
                    {
                        cardDataForSpaun = cardData;
                        res = true;
                    }
                }

                if (res == false)
                {
                    return;
                }

                Transform deckTransform = RoundManager.instance.PlayerMove == Players.Player1
                    ? m_player1DeckRoot
                    : m_player2DeckRoot;
                Card summonCard = Instantiate(m_cardPrefab, deckTransform);
                summonCard.cardManager = this;

                var _newMat = new Material(m_baseMat);
                _newMat.mainTexture = cardDataForSpaun.Texture;

                PlayerHand playerHand =
                    RoundManager.instance.PlayerMove == Players.Player1 ? _player1Hand : _player2Hand;
                summonCard.Confiruration(cardDataForSpaun, _newMat, CardUtility.GetDescriptionById(cardDataForSpaun.Id),
                    _player1Hand,
                    _player2Hand, RoundManager.instance.PlayerMove);

                summonCard.m_curParent = freeSlot.gameObject.transform;
                playerHand.AddCardOnTable(summonCard, CardState.OnTable);
                summonCard.animationComponent.AnimationFlipCard();
            }
        }

        private bool CheckDeckNull(Card[] cards)
        {
            foreach (var card in cards)
            {
                if (card != null)
                {
                    return true;
                }
            }

            return false;
        }

        public void AddCardToDeck(Card card)
        {
            Card[] arr = card.players == RoundManager.instance.PlayerMove ? m_player1Deck : m_player2Deck;
            int indx = -1;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == null)
                {
                    indx = i;
                }
            }

            if (indx != -1)
            {
                arr[indx] = card;
                card.m_cardState = CardState.InDeck;
            }
        }

        public bool CheckingCardRequirements(Card card)
        {
            PlayerData data = RoundManager.instance.PlayerMove == Players.Player1 ? m_player1Data : m_player2Data;
            if (card.Coast <= data.Mana)
            {
                data.Mana -= card.Coast;
                m_avatarScript.RefreshManaPlayer(data.Mana);
            }
            else
            {
                Debug.Log("Not enough mana");
                return false;
            }

            return true;
        }

        private (Card card, int index) GetRandomIndexFromDeck(Card[] cardsInDeck)
        {
            Card card = null;
            int index = 0;
            if (!CheckDeckNull(cardsInDeck))
            {
                return (null, index);
            }

            while (card == null)
            {
                var randomIndex = Random.Range(0, cardsInDeck.Length);
                card = cardsInDeck[randomIndex];
                index = randomIndex;
            }

            return (card, index);
        }


        private Card[] CreateDeck(Transform root, Players player)
        {
            Card[] deck = new Card[m_cardDeckCount];
            Vector3 vector = Vector3.zero;


            for (int i = 0; i < m_idForCustomDeckPlayer1.Length; i++)
            {
                deck[i] = Instantiate(m_cardPrefab, root);
                deck[i].cardManager = this;
                deck[i].transform.localPosition = vector;
                vector += new Vector3(0, c_stepCardInDeck, 0);

                CardPropertiesData card = m_allCards[0];

                if (UseCustomDeck)
                {
                    int[] customePuckCard = RoundManager.instance.PlayerMove == Players.Player1
                        ? m_idForCustomDeckPlayer1
                        : m_idForCustomDeckPlayer2;

                    foreach (var cardData in m_allCards)
                    {
                        if (cardData.Id == customePuckCard[i])
                        {
                            card = cardData;
                            break;
                        }
                    }
                }

                else
                {
                    var randomCard = m_allCards[Random.Range(0, m_allCards.Count)];
                    card = randomCard;
                }

                var _newMat = new Material(m_baseMat);
                _newMat.mainTexture = card.Texture;

                deck[i].Confiruration(card, _newMat, CardUtility.GetDescriptionById(card.Id), _player1Hand,
                    _player2Hand, player);
            }

            return deck;
        }

        public void SetEffectOnCard(Card effectOwner)
        {
            BaseEffect effect = effectOwner.GetDataCard().effect;
            if (!effect)
            {
                return;
            }
            
            effect.ApplyEffect(this);
        }

       
    }
}