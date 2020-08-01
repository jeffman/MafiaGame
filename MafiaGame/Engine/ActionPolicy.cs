using MafiaGame.Engine.Actions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MafiaGame.Engine
{
    public class ActionPolicy
    {
        public Role GetObservedRole(PlayerSlot observing, PlayerSlot observedBy)
        {
            if (observedBy.Role == Role.Host)
                return observing.Role;

            if (observing.Role == Role.Miller)
            {
                if (observedBy.Role == Role.Miller)
                    return Role.Town;

                return Role.Mafia;
            }

            if (observing.Role == Role.Godfather)
            {
                if (observedBy.Role.Alignment == Alignment.Mafia)
                    return observing.Role;

                return Role.Town;
            }

            return observing.Role;
        }

        public Alignment GetObservedAlignment(PlayerSlot observing, PlayerSlot observedBy)
        {
            return GetObservedRole(observing, observedBy).Alignment;
        }

        public ActionResult ExecuteAction(GameAction action, GameState state, out GameState newState)
        {
            newState = state;

            if (!IsActionValidForGameState(action, state))
                return new ActionResult(action, ActionOutcome.Invalid);

            return action.Execute(state, this, out newState);
        }

        public ActionResult ResolveVoteKill(GameState state, PlayerSlot toKill, GameAction originalAction, out GameState newState)
        {
            state = state.WithPlayerStatus(toKill, PlayerStatus.Dead);
            return ResolveEndOfDay(state, originalAction, out newState);
        }

        public ActionResult ResolveEndOfDay(GameState state, GameAction originalAction, out GameState newState)
        {
            newState = state.WithoutVotes().WithPhase(Phase.Night);
            return new ActionResult(originalAction, ActionOutcome.Success);
        }

        private bool IsActionValidForGameState(GameAction action, GameState state)
        {
            // Source and targets must belong to game
            if (!state.Players.Contains(action.Source) || action.Targets.Except(state.Players).Any())
                return false;

            if (!action.IsValid(state))
                return false;

            return true;
        }
    }
}
