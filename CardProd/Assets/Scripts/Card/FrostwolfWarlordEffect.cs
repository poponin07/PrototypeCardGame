using UnityEngine;

namespace Cards
{
    //Frostwolf Warlord
    [CreateAssetMenu(fileName = "FrostwolfWarlordEffect", menuName = "Effects/FrostwolfWarlordEffect")]
    public class FrostwolfWarlordEffect : BaseEffect
    {
        public int healthToAdd;
        public int attackToAdd;
        public override void ApplyEffect(CardManager cardManager, Card effectOwner)
        {
            int multiplierStats = cardManager.FrostwolfWarlordEffect();

            effectOwner.Health += multiplierStats * healthToAdd;
            effectOwner.Attack += multiplierStats * attackToAdd;
        }

        public override bool TryToRemoveEffect(CardManager cardManager)
        {
            return true;
        }
    }
}