using System;
using System.Collections.Generic;
using System.Text;

namespace MafiaGame.Engine
{
    public sealed class Player
    {
        public string Name { get; }

        public Player(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
