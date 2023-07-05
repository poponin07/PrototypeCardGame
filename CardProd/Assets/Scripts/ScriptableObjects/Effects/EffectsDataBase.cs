using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cards.ScriptableObjects
{
    [Serializable]
    public struct EffectAbilities
    {
        public int health;
        public int damage;
        public bool isPermanent;
        public CardUnitType targetType;
    }
    [Serializable]
    public class EffectParameters
    {
        public string effectName;
        public EffectsType type;
        public BaseEffect effectAbilities;
    }
}