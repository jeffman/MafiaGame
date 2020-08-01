using MafiaGame.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace MafiaGame.Engine
{
    public sealed class PlayerSlot
    {
        public Player Owner { get; }
        public Role Role { get; }
        public PlayerStatus Status { get; }

        public PlayerSlot(Player owner, Role role, PlayerStatus status)
        {
            Owner = owner;
            Role = role;
            Status = status.AsDefinedOrThrow();
        }

        public PlayerSlot(Player owner, Role role) : this(owner, role, PlayerStatus.Alive) { }

        public override string ToString()
        {
            return $"{Owner} [{Role}, {Status}]";
        }
    }
}
