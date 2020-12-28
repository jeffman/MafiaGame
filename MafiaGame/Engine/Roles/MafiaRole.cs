using System;
using System.Collections.Generic;
using System.Text;

namespace MafiaGame.Engine.Roles
{
    public class MafiaRole : Role
    {
        public override Alignment Alignment => Alignment.Mafia;
        public override Ability Ability => Ability.Vanilla;

        public MafiaRole() : base("Mafia") { }
        protected MafiaRole(string name) : base(name) { }
    }
}
