using System;
using System.Collections.Generic;
using System.Text;

namespace MafiaGame.Engine.Roles
{
    public class TownRole : Role
    {
        public override Alignment Alignment => Alignment.Town;
        public override Ability Ability => Ability.Vanilla;

        public TownRole() : base("Town") { }
    }
}
