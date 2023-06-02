using UnityEngine;

namespace Cards
{
    //Gurubashi Berserker
    [CreateAssetMenu(fileName = "BerserkerEffect", menuName = "Effects/BerserkerEffect")]
    public class BerserkerEffect : BaseEffect
    {
        public int attack;
        public override void ApplyEffect(CardManager cardManager, Card effectOwner)
        {
            if (effectOwner.effectWhenTakingDamage)
            {
                effectOwner.Attack += attack;
            }

            effectOwner.effectWhenTakingDamage = true;
        }

        public override bool TryToRemoveEffect(CardManager cardManager)
        {
            return true;
        }
    }
}