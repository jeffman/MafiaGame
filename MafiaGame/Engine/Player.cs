using MafiaGame.Utility;
using MafiaGame.Engine.Roles;
using System.Diagnostics;

namespace MafiaGame.Engine
{
    public class Player
    {
        public static Player Host { get; } = new Player(Person.Host, HostRole.Instance, PlayerStatus.Alive);

        public virtual Person Owner { get; private set; }
        public virtual Role Role { get; private set; }
        public virtual PlayerStatus Status { get; private set; }

        public Player(Person owner, Role role, PlayerStatus status)
        {
            Owner = owner;
            Role = role;
            Status = status.AsDefinedOrThrow();
        }

        public Player(Person owner, Role role) : this(owner, role, PlayerStatus.Alive) { }

        public void ChangeOwner(Person newOwner)
        {
            Debug.Assert(this != Host);
            Owner = newOwner;
        }

        public void ChangeRole(Role newRole)
        {
            Debug.Assert(this != Host);
            Role = newRole;
        }

        public void ChangeStatus(PlayerStatus newStatus)
        {
            Debug.Assert(this != Host);
            Status = newStatus.AsDefinedOrThrow();
        }

        public override string ToString()
        {
            return $"{Owner} [{Role}, {Status}]";
        }
    }
}
