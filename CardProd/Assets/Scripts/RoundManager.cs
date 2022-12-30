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
      [SerializeField]  private Players m_playerMove;
      private PlayerData m_PlayerData1;
      private PlayerData m_PlayerData2;
      private int m_manaIndex;

      public Players PlayerMove => m_playerMove;
      
      private void Awake()
      {
         if (instance != null)
            Destroy(gameObject);
         else
            instance = this;
         
         m_playerMove = Players.Player1;
         m_PlayerData1 = m_avatarScript._player1Data;
         m_PlayerData2 = m_avatarScript._player2Data;
         m_manaIndex = 1;
      }
      
      public void MoveChange()
      {
         switch (m_playerMove)
         {
            case Players.Player1:
               m_playerMove = Players.Player2;
               StartCoroutine(CoroutineTurnCamera());
               
               break;
            case Players.Player2:
               m_playerMove = Players.Player1;
               StartCoroutine(CoroutineTurnCamera());
               GetMana();
               break;
         }
      }

      private void GetMana()
      { 
         m_manaIndex += 1;
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
         }
      }
   }
}