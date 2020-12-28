using System;
using System.Collections.Generic;
using System.Text;

namespace MafiaGame.Engine.Roles
{
    public class DoctorRole : Role
    {
        public override Ability Ability => Ability.Protector;
        public override Alignment Alignment => Alignment.Town;

        public DoctorRole() : base("Doctor") { }
    }
}
