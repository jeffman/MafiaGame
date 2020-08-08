using System;
using System.Collections.Generic;
using System.Text;

namespace MafiaGame.Engine.Roles
{
    public sealed class HostRole : Role
    {
        public static HostRole Instance { get; } = new HostRole();

        public override Alignment Alignment => Alignment.Independent;
        public override Ability Ability => Ability.Vanilla;
        public override int AllowedVotes => int.MaxValue;

        private HostRole() : base("Host") { }
    }
}
