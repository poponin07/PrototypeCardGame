using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{

    public class SlotScript : MonoBehaviour
    {
        public bool couple = false;
        public int slotId;
        public int playerId;
        private Card m_cardCouple;
        public bool m_isPlyerAvatar;
        
        public Card GetCardCouple()
        {
          return m_cardCouple;
        }
        public void SwitchCouple(Card card)
        {
            m_cardCouple = card; 
            couple = !couple;
        }
    }
}