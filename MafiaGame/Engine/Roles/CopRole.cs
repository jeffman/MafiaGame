using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MafiaGame.Engine.Roles
{
    public class CopRole : Role
    {
        private class CopNightResult : NightResult
        {
            public Alignment? Result { get; }

            public CopNightResult(NightAction action, Alignment? result) : base(action, result != null)
            {
                Result = result;
            }

            protected override string GetResultStringIfSuccessful()
            {
                return $"{Action.Targets.First().Owner.Name} is a {Result!}.";
            }
        }

        public override Alignment Alignment => Alignment.Town;
        public override Ability Ability => Ability.AlignmentInvestigator;

        public CopRole() : base("Cop") { }

        public override void OnResolveNightAction(GameState state, NightAction action, NightResolver resolver)
        {
            Trace.Assert(action.Targets.Count == 1);

            if (!resolver.IsBlocked(action.Source))
            {
                var intendedTarget = action.Targets.First();
                var actualTarget = resolver.GetActualTarget(intendedTarget);
                var alignment = actualTarget.Role.OnInvestigateAlignment(state, actualTarget, action.Source);
                resolver.AddResult(new CopNightResult(action, alignment));
            }
            else
            {
                resolver.AddResult(new CopNightResult(action, null));
            }
        }
    }
}
