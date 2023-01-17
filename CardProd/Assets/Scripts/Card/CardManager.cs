﻿
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

        public Transform m_player1DeckRoot;
        public Transform m_player2DeckRoot;

        [SerializeField, Space] private PlayerHand _player1Hand;
        [SerializeField] private PlayerHand _player2Hand;

        private Card[] m_player1Deck;
        private Card[] m_player2Deck;

        private const float c_stepCardInDeck = 0.07f;
        private Material m_baseMat; 

        private Dictionary<int, SlotScript> m_playerCardSlots = new Dictionary<int, SlotScript>();
        
        [SerializeField] private PlayerCardSlotsManager m_slotsManager;
        public List<SlotScript> cardsOntablePlayer1;
        public List<SlotScript> cardsOntablePlayer2;
        private UIAvatarScript m_avatarScript;
        public PlayerData m_player1Data;
        public PlayerData m_player2Data;
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
                        cardsOntablePlayer1.Add(cardSlot.Value);
                    }
                    else
                    {
                        cardsOntablePlayer2.Add(cardSlot.Value);
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

        public void GetCardFromDeck(int cards, bool isRandomCard)
        {
            for (int j = cards; j != 0; j--)
            {
                int index;
                Card[] playerDeck;

                switch (RoundManager.instance.PlayerMove)
                {
                    case Players.Player1:
                        playerDeck = m_player1Deck;
                        index = -1;
                        break;
                    case Players.Player2:
                        playerDeck = m_player2Deck;
                        index = -1;
                        break;
                    default:
                        playerDeck = m_player1Deck;
                        index = -1;
                        break;
                }

                if (isRandomCard)
                {
                    index = GetRandomIndexFromDeck();
                }
                
                if (!isRandomCard || index == -1)
                {
                    for (int i = playerDeck.Length - 1; i >= 0; i--)
                    {
                        if (playerDeck[i] != null)
                        {
                            index = i;
                            break;
                        }
                    } 
                }

                bool resultSetNewCard;
                
                if (m_player1Deck[index] != null)
                {
                    switch (RoundManager.instance.PlayerMove)
                    {
                        case Players.Player1:
                            resultSetNewCard = _player1Hand.SetNewCardInHand(m_player1Deck[index], true);
                            if (resultSetNewCard)
                            {
                                m_player1Deck[index] = null;
                            }
                            break;

                        case Players.Player2:
                            resultSetNewCard = _player2Hand.SetNewCardInHand(m_player2Deck[index], true);
                            if (resultSetNewCard) m_player2Deck[index] = null;
                            break;
                    }
                }
                else
                {
                    
                }
            }
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
            if (card.coast <= data.Mana)
            {
                data.Mana -= card.coast;
                m_avatarScript.RefreshManaPlayer(data.Mana);
            }
            else
            {
                Debug.Log("Not enough mana");
                return false;
            }
            return true;
        }

       /* public Card[] GenStartPoolCardPack(Players _player)
        {
            Card[] startPoolCardPack = new Card[] { };
            List<int> expInt = null;
            for (int i = 0; i < 3; i++)
            {
                int inx = Random.Range(0, m_player1Deck.Length);
                if (expInt.Contains(inx))
                {
                    i--;
                }
                else
                {
                    if (_player == Players.Player1)
                    {
                        startPoolCardPack[i] = m_player1Deck[inx];
                    }
                    else
                    {
                        startPoolCardPack[i] = m_player2Deck[inx];
                    }
                }
            

            return startPoolCardPack;
        }*/

       private int GetRandomIndexFromDeck()
       {
           Card[] cardsInDeck = RoundManager.instance.PlayerMove == Players.Player1 ? m_player1Deck : m_player2Deck;
           int indx = Random.Range(0, cardsInDeck.Length);
           return indx;
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