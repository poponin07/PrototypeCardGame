using UnityEngine;

namespace Cards
{
    //дает карту из колоды
    [CreateAssetMenu(fileName = "BattleCryDrawCard", menuName = "Effects/BattleCryDrawCard")]
    public class BattleCryDrawCard : BaseEffect
    {
        public override void ApplyEffect(CardManager cardManager, Card effectOwner)
        {
            cardManager.GetCardFromDeck(1, true);
        }

        public override bool TryToRemoveEffect(CardManager cardManager)
        {
            return true;
        }
    }
}