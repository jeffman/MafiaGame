using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MafiaGame.Engine;
using Xunit;

namespace MafiaGameTest.Engine.ActionPolicyTest
{
    public class RoleTest : ActionPolicyTest
    {
        [Theory]
        [MemberData(nameof(Slots))]
        public void HostAlwaysObservesActualRole(PlayerSlot observing)
        {
            Assert.Same(observing.Role, Policy.GetObservedRole(observing, PlayerSlot.Host));
        }

        [Theory]
        [MemberData(nameof(NonMillerSlots))]
        public void EveryoneThinksMillerIsMafia(PlayerSlot observedBy)
        {
            Assert.Same(Role.Mafia, Policy.GetObservedRole(Utility.OddSlots.Miller, observedBy));
        }

        [Fact]
        public void MillerThinksTheyAreTown()
        {
            Assert.Same(Role.Town, Policy.GetObservedRole(Utility.OddSlots.Miller, Utility.OddSlots.Miller));
        }
        
        [Theory]
        [MemberData(nameof(TownSlots))]
        public void TownThinksGodfatherIsTown(PlayerSlot observedBy)
        {
            Assert.Same(Role.Town, Policy.GetObservedRole(Utility.MafiaSlots.Gabby, observedBy));
        }

        [Theory]
        [MemberData(nameof(MafiaSlots))]
        public void MafiaSeesGodfather(PlayerSlot observedBy)
        {
            Assert.Same(Role.Godfather, Policy.GetObservedRole(Utility.MafiaSlots.Gabby, observedBy));
        }
    }
}
