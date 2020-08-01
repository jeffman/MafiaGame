using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MafiaGame.Engine;

namespace MafiaGameTest.Engine.ActionPolicyTest
{
    public class ActionPolicyTest
    {
        public static ActionPolicy Policy { get; } = new ActionPolicy();
        public static GameState State { get; }

        public static IEnumerable<object[]> Slots => State.Players.AsMemberData();
        public static IEnumerable<object[]> NonMillerSlots => State.Players
            .Where(p => p != Utility.OddSlots.Miller && p != PlayerSlot.Host)
            .AsMemberData();
        public static IEnumerable<object[]> MafiaSlots => State.Players
            .Where(p => p != PlayerSlot.Host && p.Role.Alignment == Alignment.Mafia)
            .AsMemberData();
        public static IEnumerable<object[]> TownSlots => State.Players
            .Where(p => p != PlayerSlot.Host && p.Role.Alignment == Alignment.Town)
            .AsMemberData();

        static ActionPolicyTest()
        {
            State = GameState.Blank.WithPlayers(new[]
            {
                PlayerSlot.Host,
                Utility.TownSlots.Alice,
                Utility.TownSlots.Bob,
                Utility.TownSlots.Chris,
                Utility.TownSlots.Denise,
                Utility.MafiaSlots.Gabby,
                Utility.MafiaSlots.James,
                Utility.MafiaSlots.Kelly,
                Utility.MafiaSlots.Larry,
                Utility.OddSlots.Miller
            });
        }
    }
}
