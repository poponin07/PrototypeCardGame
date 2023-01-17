using System;
using Cards;
using UnityEngine;

namespace Player
{
    public class PlayerScript: MonoBehaviour
    {
        private PlayerData m_plaerData;
        [SerializeField] private UIAvatarScript m_UIavatarscript;
        private int  m_damageCounterForCards;

        private void Awake()
        {
            m_plaerData = GetComponent<PlayerData>();
            m_damageCounterForCards = 1;
        }

        public bool GetDamage(int damage,bool  forGetCard)
        {
            if (forGetCard)
            {
                m_plaerData.Health -= m_damageCounterForCards;
                m_damageCounterForCards++;
            }
            else
            {
                m_plaerData.Health -= damage; 
            }
            
            
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