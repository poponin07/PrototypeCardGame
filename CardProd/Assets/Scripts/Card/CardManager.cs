
using System;
using System.Collections.Generic;
using Cards.ScriptableObjects;
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
        
        [SerializeField] private Transform[] m_positonsCardOnTablePlayer1;
        [SerializeField] private Transform[] m_positonsCardOnTablePlayer2;

        private Card[] m_player1Deck;
        private Card[] m_player2Deck;

        private const float c_stepCardInDeck = 0.07f;
        private Material m_baseMat;

        public Transform[] MPositonsCardOnTablePlayer1 => m_positonsCardOnTablePlayer1;
        public Transform[] MPositonsCardOnTablePlayer2 => m_positonsCardOnTablePlayer2;


        private void Awake()
        {
            CollectingAllCards();
        }

        private void Start()
        {
            m_player1Deck = CreateDeck(m_player1DeckRoot, Players.Player1);
            m_player2Deck = CreateDeck(m_player2DeckRoot, Players.Player2);
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
            switch (RoundManager.instance.PlayerMove)
            {
                case Players.Player1:
                    index = m_player1Deck.Length - 1;
                    for (int i = index; i >= 0; i--)
                    {
                        if (m_player1Deck[i] != null)
                        {
                            index = i;
                            break;
                        }
                    }
                    break;
                case Players.Player2:
                    index = m_player2Deck.Length - 1;
                    for (int i = index; i >= 0; i--)
                    {
                        if (m_player2Deck[i] != null)
                        {
                            index = i;
                            break;
                        }
                    }
                    break;
                default:
                    return;
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
                case Players.Discard:
                    break;
            }
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

                var random = m_allCards[Random.Range(0, m_allCards.Count)];

                var _newMat = new Material(m_baseMat);
                _newMat.mainTexture = random.Texture;

                deck[i].Confiruration(random, _newMat, CardUtility.GetDescriptionById(random.Id), _player1Hand, player);
                deck[i].m_cardState = CardState.InDeck;
                deck[i].SwitchEnable();
            }

            return deck;
        }
    }
}