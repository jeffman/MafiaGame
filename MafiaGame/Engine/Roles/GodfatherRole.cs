using System;
using System.Collections.Generic;
using System.Text;

namespace MafiaGame.Engine.Roles
{
    public class GodfatherRole : MafiaRole
    {
        public GodfatherRole() : base("Godfather") { }

        public override Alignment OnInvestigateAlignment(GameState state, Player owner, Player investigator)
        {
            if (state.Resolution.IsBlocked(owner))
                return Alignment.Mafia;

            return Alignment.Town;
        }
    }
}
