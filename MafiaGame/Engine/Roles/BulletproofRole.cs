using System;
using System.Collections.Generic;
using System.Text;

namespace MafiaGame.Engine.Roles
{
    public class BulletproofRole : Role
    {
        public override Alignment Alignment { get; }
        public override Ability Ability => Ability.Bulletproof;

        public BulletproofRole(Alignment alignment) : base("Bulletproof")
        {
            Alignment = alignment;
        }

        public override void OnResolveNightPassiveAction(GameState state, Player owner, NightResolver resolver)
        {
            resolver.Protect(owner);
        }
    }
}
