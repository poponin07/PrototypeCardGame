using System;
using System.Collections.Generic;
using OneLine;
using UnityEngine;

namespace Cards.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CardPack", menuName = "CreateCardPack", order = 0)]
    public class CardPack : ScriptableObject
    {
        [SerializeField, OneLine(Header = LineHeader.Short)]
        public List<CardPropertiesData> ids = new List<CardPropertiesData>();
        public CardPropertiesData GetCardById(uint id)
        {
            CardPropertiesData card = ids.Find(c => c.Id == id);
            return card;
        }
    }
}