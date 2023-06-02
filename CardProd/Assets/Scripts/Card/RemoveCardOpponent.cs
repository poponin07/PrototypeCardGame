using UnityEngine;

namespace Cards
{
    //удаляет карту соперника
    [CreateAssetMenu(fileName = "RemoveCardOpponent", menuName = "Effects/RemoveCardOpponent")]
    public class RemoveCardOpponent : BaseEffect
    {
        public override void ApplyEffect(CardManager cardManager, Card effectOwner)
        {
            cardManager.RemoveCardOpponent();
        }

        public override bool TryToRemoveEffect(CardManager cardManager)
        {
            return true;
        }
    }
}