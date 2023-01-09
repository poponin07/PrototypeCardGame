using System;
using Cards;
using UnityEngine;

namespace Player
{
    public class PlayerScript: MonoBehaviour
    {
        private PlayerData m_plaerData;
        [SerializeField] private UIAvatarScript m_UIavatarscript;

        private void Awake()
        {
            m_plaerData = GetComponent<PlayerData>();

        }

        public bool GetDamage(int damage)
        {
            m_plaerData.Health -= damage;
            
            m_UIavatarscript.RefreshHealthPlayer(m_plaerData.Health);
            
            if (m_plaerData.Health <= 0)
            {
                m_UIavatarscript.RefreshHealthPlayer(0);
                return true;
            }
            
            return false;
        }
    }
}