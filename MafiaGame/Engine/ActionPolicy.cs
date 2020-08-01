using System;
using System.Collections.Generic;
using System.Text;

namespace MafiaGame.Engine
{
    public class ActionPolicy
    {
        public Role GetObservedRole(PlayerSlot observing, PlayerSlot observedBy)
        {
            return observing.Role;
        }

        public ActionResult ExecuteAction(GameAction action, GameState state, out GameState newState)
        {
            newState = state;

            if (!IsActionValidForGameState(action, state))
                return new ActionResult(action, ActionOutcome.Invalid);

            return new ActionResult(action, ActionOutcome.Success);
        }

        private bool IsActionValidForGameState(GameAction action, GameState state)
        {
            return true;
        }
    }
}
