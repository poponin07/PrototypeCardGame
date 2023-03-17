using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cards.ScriptableObjects
{
    public class ScriptableObjectLoader : MonoBehaviour
    {
        private CardPack m_cardPack;
        public const string SCRIPTABLE_OBJECTS_PATH = "ScriptableObjects/";
      private void Awake()
        {
            m_cardPack = Resources.Load<CardPack>(SCRIPTABLE_OBJECTS_PATH + "CardPack");
            string constName = "-CommonCardPack";
            List<CardPackConfiguration> cardPacks = new List<CardPackConfiguration>(); 
            for (int i = 1; i <= 7; i++)
            {
                cardPacks.Add(Resources.Load<CardPackConfiguration>(SCRIPTABLE_OBJECTS_PATH + i + constName));
            }

            for (int i = 0; i < cardPacks.Count; i++)
            {
                var cardPack = cardPacks[i];
                m_cardPack.ids.AddRange(cardPack._cards);
            }
            CardPropertiesData card = m_cardPack.GetCardById(0);
        }
    }
}