using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MafiaGame.Engine;

namespace MafiaGameTest.Engine
{
    public static class Utility
    {
        public static class Players
        {
            public static Player Alice { get; } = new Player("Alice");
            public static Player Bob { get; } = new Player("Bob");
            public static Player Chris { get; } = new Player("Chris");
            public static Player Denise { get; } = new Player("Denise");

            public static Player Gabby { get; } = new Player("Gabby");
            public static Player James { get; } = new Player("James");
            public static Player Kelly { get; } = new Player("Kelly");
            public static Player Larry { get; } = new Player("Larry");

            public static Player Miller { get; } = new Player("Miller");
        }

        public static class TownSlots
        {
            public static PlayerSlot Alice { get; } = new PlayerSlot(Players.Alice, Role.Town);
            public static PlayerSlot Bob { get; } = new PlayerSlot(Players.Bob, Role.Town);
            public static PlayerSlot Chris { get; } = new PlayerSlot(Players.Chris, Role.Town);
            public static PlayerSlot Denise { get; } = new PlayerSlot(Players.Denise, Role.Town);
        }

        public static class MafiaSlots
        {
            public static PlayerSlot Gabby { get; } = new PlayerSlot(Players.Gabby, Role.Godfather);
            public static PlayerSlot James { get; } = new PlayerSlot(Players.James, Role.Mafia);
            public static PlayerSlot Kelly { get; } = new PlayerSlot(Players.Kelly, Role.Mafia);
            public static PlayerSlot Larry { get; } = new PlayerSlot(Players.Larry, Role.Mafia);
        }

        public static class OddSlots
        {
            public static PlayerSlot Miller { get; } = new PlayerSlot(Players.Miller, Role.Miller);
        }

        public static IEnumerable<object[]> AsMemberData(this IEnumerable<object> data)
        {
            return data.Select(d => new[] { d });
        }
    }
}
