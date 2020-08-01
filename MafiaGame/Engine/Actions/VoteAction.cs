using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MafiaGame.Engine.Actions
{
    public class VoteAction : GameAction
    {
        public Vote Vote { get; }

        public VoteAction(PlayerSlot source, PlayerSlot target) : base(source, target)
        {
            Vote = new Vote(source, target);
        }

        public override bool IsValid(GameState state)
        {
            if (Vote.Target.Status != PlayerStatus.Alive)
                return false;

            if (state.Phase != Phase.Day)
                return false;

            return true;
        }

        public override ActionResult Execute(GameState state, ActionPolicy policy, out GameState newState)
        {
            newState = state;

            if (state.Votes.Any(v => v.Voter == Source))
                return new ActionResult(this, ActionOutcome.NotAllowed);

            state = state.WithVote(Vote);
            var newVotes = state.Votes;

            // Majority reached?
            int majorityCount = state.ActivePlayers.Count / 2 + 1;
            var votesByTarget = newVotes.GroupBy(v => v.Target);
            var targetsWithMajority = votesByTarget.Where(g => g.Count() >= majorityCount).ToList();
            Debug.Assert(targetsWithMajority.Count < 2);
            if (targetsWithMajority.Count == 1)
            {
                var target = targetsWithMajority.First().Key;
                Debug.Assert(target == Vote.Target);
                return policy.ResolveVoteKill(state, target, this, out newState);
            }

            // Everyone has voted?
            bool hasEveryoneVoted = state.ActivePlayers.All(p => newVotes.Any(v => v.Voter == p));
            if (hasEveryoneVoted)
            {
                return policy.ResolveEndOfDay(state, this, out newState);
            }

            newState = state;
            return new ActionResult(this, ActionOutcome.Success);
        }
    }
}
