using System;
using System.Collections.Generic;
using System.Text;

namespace MafiaGame.Engine
{
    public sealed class Redirection
    {
        public Player IntendedTarget { get; }
        public Player ActualTarget { get; }

        public Redirection(Player intended, Player actual)
        {
            IntendedTarget = intended;
            ActualTarget = actual;
        }
    }
}
