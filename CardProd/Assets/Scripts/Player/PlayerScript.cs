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

        public void GetDamage(int damage,bool  forGetCard)
        {
            if (forGetCard)
            {
                m_plaerData.Health -= m_damageCounterForCards;
                m_damageCounterForCards++;
                m_UIavatarscript.RefreshHealthPlayer(m_plaerData.Health, false);
            }
            else
            {
                m_plaerData.Health -= damage; 
                m_UIavatarscript.RefreshHealthPlayer(m_plaerData.Health, true);
            }

            if ( m_plaerData.Health <= 0 )
            {
                Debug.LogError(RoundManager.instance.PlayerMove + " wins!");
            }
        }

        public void RestoreHealth(int restoreData)
        {
            m_plaerData.Health += restoreData;
            if (m_plaerData.Health > m_plaerData.HealthDefault)
            {
                m_plaerData.Health = m_plaerData.HealthDefault;
            }
            m_UIavatarscript.RefreshHealthPlayer(m_plaerData.Health, false);
        }
        
        public int GetDamageForEmptyDeck()
        {
            return m_damageCounterForCards;
        }
    }
}