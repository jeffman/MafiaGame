using System;
using System.Collections.Generic;
using System.Text;

namespace MafiaGame.Engine.Roles
{
    public class MillerRole : TownRole
    {
        public override Alignment OnInvestigateAlignment(GameState state, Player owner, Player investigator)
        {
            if (state.Resolution.IsBlocked(owner))
                return Alignment;

            return Alignment.Mafia;
        }
    }
}
