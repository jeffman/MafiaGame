using System;
using System.Collections.Generic;
using System.Text;

namespace MafiaGame.Engine
{
    public class Vote
    {
        public Player Voter { get; }
        public Player Target { get; }

        public Vote(Player voter, Player target)
        {
            Voter = voter;
            Target = target;
        }

        public override string ToString()
        {
            return $"Vote: {Target}";
        }
    }
}
