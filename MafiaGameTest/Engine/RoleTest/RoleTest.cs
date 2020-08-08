using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MafiaGame.Engine;
using MafiaGame.Engine.Roles;
using Xunit;

namespace MafiaGameTest.Engine.ActionPolicyTest
{
    public class RoleTest : PlayerBase
    {
        [Fact]
        public void GodfatherInvestiatesAsTown()
        {
            foreach (var player in state.Players)
                Assert.Equal(Alignment.Town, gabby.Role.OnInvestigateAlignment(state, gabby, player));
        }

        [Fact]
        public void MillerInvestigatesAsMafia()
        {
            foreach (var player in state.Players)
                Assert.Equal(Alignment.Mafia, millhouse.Role.OnInvestigateAlignment(state, millhouse, player));
        }
    }
}
