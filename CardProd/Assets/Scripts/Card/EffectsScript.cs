﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cards;
using UnityEditor;
using UnityEngine;

namespace Cards
{
    [Serializable]
    public abstract class BaseEffect : ScriptableObject
    {
        public Card Parent;
        public bool Permanent;
        public CardUnitType type;

        public abstract void ApplyEffect(Card target);

        public abstract bool TryToRemoveEffect(Card target);
    }



    [CreateAssetMenu(fileName = "StatsEffect", menuName = "Effects/StatsEffect")]
    public class StatsEffect : BaseEffect
    {
        public int Damage;
        public int Health;

        public override void ApplyEffect(Card target)
        {
            target.health += Health;
            target.attack += Damage;
        }

        public override bool TryToRemoveEffect(Card target)
        {
            if (Permanent) return false;

            target.health -= Health;
            target.attack -= Damage;

            return true;
        }


    }

    [CreateAssetMenu(fileName = "BattleCry", menuName = "Effects/BattleCry")]
    public class BattleCry : BaseEffect
    {
        public override void ApplyEffect(Card target)
        {
            throw new System.NotImplementedException();
        }

        public override bool TryToRemoveEffect(Card target)
        {
            throw new System.NotImplementedException();
        }
    }
}

        