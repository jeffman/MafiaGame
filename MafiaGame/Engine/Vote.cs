using System;
using System.Collections.Generic;
using System.Text;

namespace MafiaGame.Engine
{
    public class Vote
    {
        public PlayerSlot Voter { get; }
        public PlayerSlot Target { get; }

        public Vote(PlayerSlot voter, PlayerSlot target)
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
