using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEngine;


    public abstract class BaseAbilities : ScriptableObject
    {
        public CardManager m_cardManager;
        public abstract void ApplyAbility();
    }