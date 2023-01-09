using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class PlayerData : MonoBehaviour
    {
        [SerializeField] private int m_health;
        [SerializeField] private int m_mana;
        [SerializeField] public Players m_players;
        

        public int Health
        {
            get => m_health;
            set => m_health = value;
        }

        public int Mana
        {
            get => m_mana;
            set => m_mana = value;
        }
    }
}