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

      public Players PlayerMove => m_playerMove;
      
      private void Awake()
      {
         ferstMoveIsDonePlayerIndex = 4;
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
      //смена флага возможности ходить
      public void SetIndexIsReadyMome(List<SlotScript> cardSlot)
      {
         foreach (var slot in cardSlot)
         {
           Card card = slot.GetCardCouple();
           card?.RefresMoveIndex(-1);
         }
      }
      
      //раздача карт по раундам
      public void DistributionCards()
      {
         if (ferstMoveIsDonePlayerIndex > 2)
         {
            --ferstMoveIsDonePlayerIndex;
            return;
         }
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
      
      //ход
      public void MoveChange()
      {
         switch (m_playerMove)
         {
            case Players.Player1:
               m_playerMove = Players.Player2;
               SetIndexIsReadyMome(m_cardManager.slotsOntablePlayer1);
               StartCoroutine(CoroutineTurnCamera());
               
               break;
            case Players.Player2:
               m_playerMove = Players.Player1;
               SetIndexIsReadyMome(m_cardManager.slotsOntablePlayer2);
               StartCoroutine(CoroutineTurnCamera());
               GetMana();
               break;
         }
      }

      //разадача маны
      private void GetMana()
      {
         if (ferstMoveIsDonePlayerIndex > 2)
         {
            return;
         }
         if (m_manaIndex < 10)
         {
            m_manaIndex++;
         }
         
         m_PlayerData1.Mana = m_manaIndex;
        m_PlayerData2.Mana = m_manaIndex;
        
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