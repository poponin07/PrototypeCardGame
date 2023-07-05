using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Cards
{
    public class PlayerData : MonoBehaviour
    {
        private int healthDefault;
        [SerializeField] private int m_health;
        [SerializeField] private int m_mana;
        //[SerializeField] public Players m_players;

        private void Start()
        {
            healthDefault = m_health;
        }

        public int HealthDefault
        {
            get => healthDefault;
        }
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