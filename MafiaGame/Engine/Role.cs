using MafiaGame.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace MafiaGame.Engine
{
    public class Role
    {
        public static Role Host { get; } = new Role(Alignment.Independent, "Host");
        public static Role Town { get; } = new Role(Alignment.Town, "Town");
        public static Role Mafia { get; } = new Role(Alignment.Mafia, "Mafia");
        public static Role Godfather { get; } = new Role(Alignment.Mafia, "Godfather");
        public static Role Miller { get; } = new Role(Alignment.Town, "Miller");

        public Alignment Alignment { get; }
        public string Name { get; }

        private Role(Alignment alignment, string name)
        {
            Alignment = alignment.AsDefinedOrThrow();
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
