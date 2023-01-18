using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
   public class RoundManager : MonoBehaviour
   {
      public static RoundManager instance;
      
      [SerializeField, Range(0.1f, 100f)] private float speedRotateCamera;
      [SerializeField] public UIAvatarScript m_avatarScript;
      [SerializeField] private Players m_playerMove;

      [SerializeField] private CardManager m_cardManager;
      private PlayerData m_PlayerData1;
      private PlayerData m_PlayerData2;
      private int m_manaIndex;
      private int ferstMoveIsDonePlayerIndex;
      //public event Action EndRound;
      
      public Players PlayerMove => m_playerMove;
      
      private void Awake()
      {
         ferstMoveIsDonePlayerIndex = 2;
         if (instance != null)
            Destroy(gameObject);
         else
            instance = this;
         
         m_playerMove = Players.Player1;
         m_PlayerData1 = m_avatarScript._player1Data;
         m_PlayerData2 = m_avatarScript._player2Data;
         m_manaIndex = 0;
         m_avatarScript.RefreshPlayerManaRound(m_PlayerData1.Mana, m_PlayerData2.Mana);
      }

      private void Start()
      {
         DistributionCards();
      }
      public void SetIndexIsReadyMome(List<SlotScript> cardSlot)
      {
         foreach (var slot in cardSlot)
         {
           Card card = slot.GetCardCouple();
           card?.RefresMoveIndex(-1);
         }
      }
      public void DistributionCards()
      {
         if (ferstMoveIsDonePlayerIndex != 0)
         {
            m_cardManager.GetCardFromDeck(3, true);
            --ferstMoveIsDonePlayerIndex;
         }
         else
         {
            m_cardManager.GetCardFromDeck(1, false);
         }
      }
      
      public void MoveChange()
      {
         switch (m_playerMove)
         {
            case Players.Player1:
               m_playerMove = Players.Player2;
               SetIndexIsReadyMome(m_cardManager.cardsOntablePlayer1);
               StartCoroutine(CoroutineTurnCamera());
               
               break;
            case Players.Player2:
               m_playerMove = Players.Player1;
               SetIndexIsReadyMome(m_cardManager.cardsOntablePlayer2);
               StartCoroutine(CoroutineTurnCamera());
               GetMana();
               break;
         }
      }

      public void EndFerstMove()
      {
         if (ferstMoveIsDonePlayerIndex > 0)
         {
            ferstMoveIsDonePlayerIndex--;
         }
      }

      private void GetMana()
      {
         m_manaIndex++;
         m_PlayerData1.Mana += m_manaIndex;
        m_PlayerData2.Mana += m_manaIndex;
        
         m_avatarScript.RefreshPlayerManaRound(m_PlayerData1.Mana, m_PlayerData2.Mana);
      }

      public IEnumerator CoroutineTurnCamera() //корутина поворота камеры
      {
         m_avatarScript.StartCoroutine(m_avatarScript.CoroutineTurnIcon());
         if (Time.timeScale != 0)
         {
            float angle = 0;
            while (angle < 180)
            {
               float deltaAngle = speedRotateCamera * Time.deltaTime;
               transform.Rotate(Vector3.up, deltaAngle);
               angle += deltaAngle;
               yield return null;
            }
            DistributionCards();
         }
      }
   }
}