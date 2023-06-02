using UnityEngine;

namespace Cards
{
    //восстанавливает доровье 
    [CreateAssetMenu(fileName = "BattleCryRestoreHealth", menuName = "Effects/BattleCryRestoreHealth")]
    public class BattleCryRestoreHealth : BaseEffect
    {
        public int valueHealthRestore;

        public override void ApplyEffect(CardManager cardManager, Card effectOwner)
        {
            if (RoundManager.instance.PlayerMove == Players.Player1)
            {
                cardManager.player1Script.RestoreHealth(valueHealthRestore);
            }
            else
            {
                cardManager.player2Script.RestoreHealth(valueHealthRestore);
            }
        }

        public override bool TryToRemoveEffect(CardManager cardManager)
        {
            return true;
        }
    }
}