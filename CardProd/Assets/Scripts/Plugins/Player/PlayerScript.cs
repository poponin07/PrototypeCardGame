using System;
using Cards;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerScript: MonoBehaviour
    {
        private PlayerData m_playerData;
        [SerializeField] private UIAvatarScript m_UIavatarscript;
        [SerializeField] private Button m_abilityButtonButton;
        private int  m_damageCounterForCards;
        public BaseAbilities m_ability;
        

        private void Awake()
        {
            m_playerData = GetComponent<PlayerData>();
            m_damageCounterForCards = 1;
        }

        public void ApplyAbility()
        {
            m_ability.ApplyAbility();
            m_abilityButtonButton.enabled = false;
        }

        
        public void SetHeroParams(BaseAbilities ability)
        {
            m_ability = ability;
        }
        
        //игрок получает урон
        public void GetDamage(int damage,bool  forGetCard)
        {
            if (forGetCard)
            {
                m_playerData.Health -= m_damageCounterForCards;
                m_damageCounterForCards++;
                m_UIavatarscript.RefreshHealthPlayer(m_playerData.Health, false);
            }
            else
            {
                m_playerData.Health -= damage; 
                m_UIavatarscript.RefreshHealthPlayer(m_playerData.Health, true);
            }

            if ( m_playerData.Health <= 0 )
            {
                Debug.LogError(RoundManager.instance.PlayerMove + " wins!");
            }
        }

        //восстановление здоровья
        public void RestoreHealth(int restoreData)
        {
            m_playerData.Health += restoreData;
            if (m_playerData.Health > m_playerData.HealthDefault)
            {
                m_playerData.Health = m_playerData.HealthDefault;
            }
            m_UIavatarscript.RefreshHealthPlayer(m_playerData.Health, false);
        }
        
        //игрок получает урон из-за пустой колоды
        public int GetDamageForEmptyDeck()
        {
            return m_damageCounterForCards;
        }
    }
}