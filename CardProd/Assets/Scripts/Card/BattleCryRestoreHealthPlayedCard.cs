using UnityEngine;

namespace Cards
{
    //восстанавливает доровье 
    [CreateAssetMenu(fileName = "BattleCryRestoreHealthPlayedCard", menuName = "Effects/BattleCryRestoreHealthPlayedCard")]
    public class BattleCryRestoreHealthPlayedCard : BaseEffect
    {
        public int valueHealthRestore;

        public override void ApplyEffect(CardManager cardManager, Card effectOwner)
        {

            cardManager.RestoreHealthCharacters(valueHealthRestore);
        }

        public override bool TryToRemoveEffect(CardManager cardManager)
        {
            return true;
        }
    }
}