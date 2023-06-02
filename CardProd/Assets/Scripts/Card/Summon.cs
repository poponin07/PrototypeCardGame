using UnityEngine;

namespace Cards
{
    //призыв миньона
    [CreateAssetMenu(fileName = "Summon", menuName = "Effects/Summon")]
    public class Summon : BaseEffect
    {
        public int cardIdToSummon;
        public override void ApplyEffect(CardManager cardManager, Card effectOwner)
        {
            cardManager.AddSummonCardOntable(cardIdToSummon);
        }

        public override bool TryToRemoveEffect(CardManager cardManager)
        {
            return true;
        }
    }
}