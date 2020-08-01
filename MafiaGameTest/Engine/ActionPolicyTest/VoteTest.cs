using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MafiaGame.Engine;
using MafiaGame.Engine.Actions;
using Xunit;

namespace MafiaGameTest.Engine.ActionPolicyTest
{
    public class VoteTest : ActionPolicyTest
    {
        [Fact]
        public void CanVoteAtLeastOnce()
        {
            var state = State.WithPhase(Phase.Day);
            var action = new VoteAction(Utility.TownSlots.Alice, Utility.TownSlots.Bob);
            var result = Policy.ExecuteAction(action, state, out var newState);
            Assert.Equal(ActionOutcome.Success, result.Outcome);
            Assert.Same(action, result.Action);
            Assert.Equal(1, newState.Votes.Count);
            Assert.Same(action.Targets.First(), newState.Votes.First().Target);
            Assert.Same(action.Source, newState.Votes.First().Voter);
        }

        [Fact]
        public void CannotVoteTwice()
        {
            var state = State.WithPhase(Phase.Day);
            var action = new VoteAction(Utility.TownSlots.Alice, Utility.TownSlots.Bob);
            var result = Policy.ExecuteAction(action, state, out var newState);
            result = Policy.ExecuteAction(action, newState, out newState);
            Assert.Equal(ActionOutcome.NotAllowed, result.Outcome);
            Assert.Same(action, result.Action);
            Assert.Equal(1, newState.Votes.Count);
        }

        [Fact]
        public void CannotVoteAtNight()
        {
            var action = new VoteAction(Utility.TownSlots.Alice, Utility.TownSlots.Bob);
            var result = Policy.ExecuteAction(action, State, out var newState);
            Assert.Equal(ActionOutcome.Invalid, result.Outcome);
            Assert.Equal(0, newState.Votes.Count);
        }
        
        [Fact]
        public void MajorityVoteKillsPlayer()
        {
            var state = State.WithPhase(Phase.Day);
            DoFiveVotes(state);
        }

        [Fact]
        public void MajorityOfEvenPlayersIsHalfPlusOne()
        {
            var state = State.WithPhase(Phase.Day).WithoutPlayer(Utility.MafiaSlots.James);
            DoFiveVotes(state);
        }

        private void DoFiveVotes(GameState state)
        {
            Policy.ExecuteAction(new VoteAction(Utility.TownSlots.Alice, Utility.OddSlots.Miller), state, out var newState);
            Policy.ExecuteAction(new VoteAction(Utility.TownSlots.Bob, Utility.OddSlots.Miller), newState, out newState);
            Policy.ExecuteAction(new VoteAction(Utility.TownSlots.Chris, Utility.OddSlots.Miller), newState, out newState);
            Policy.ExecuteAction(new VoteAction(Utility.TownSlots.Denise, Utility.OddSlots.Miller), newState, out newState);
            Assert.Equal(Phase.Day, newState.Phase);

            Policy.ExecuteAction(new VoteAction(Utility.MafiaSlots.Gabby, Utility.OddSlots.Miller), newState, out newState);
            Assert.Equal(Phase.Night, newState.Phase);
            Assert.Equal(PlayerStatus.Dead, newState.GetPlayerFromOwner(Utility.Players.Miller)!.Status);
            Assert.Equal(0, newState.Votes.Count);
        }

        [Fact]
        public void DayEndsWhenEveryoneVotes()
        {
            var state = State.WithPhase(Phase.Day);
            Policy.ExecuteAction(new VoteAction(Utility.TownSlots.Alice, Utility.TownSlots.Alice), state, out var newState);
            Policy.ExecuteAction(new VoteAction(Utility.TownSlots.Bob, Utility.TownSlots.Bob), newState, out newState);
            Policy.ExecuteAction(new VoteAction(Utility.TownSlots.Chris, Utility.TownSlots.Chris), newState, out newState);
            Policy.ExecuteAction(new VoteAction(Utility.TownSlots.Denise, Utility.TownSlots.Denise), newState, out newState);
            Policy.ExecuteAction(new VoteAction(Utility.MafiaSlots.Gabby, Utility.MafiaSlots.Gabby), newState, out newState);
            Policy.ExecuteAction(new VoteAction(Utility.MafiaSlots.James, Utility.MafiaSlots.James), newState, out newState);
            Policy.ExecuteAction(new VoteAction(Utility.MafiaSlots.Kelly, Utility.MafiaSlots.Kelly), newState, out newState);
            Policy.ExecuteAction(new VoteAction(Utility.MafiaSlots.Larry, Utility.MafiaSlots.Larry), newState, out newState);
            Assert.Equal(Phase.Day, newState.Phase);

            Policy.ExecuteAction(new VoteAction(Utility.OddSlots.Miller, Utility.OddSlots.Miller), newState, out newState);
            Assert.Equal(Phase.Night, newState.Phase);
            Assert.Equal(0, newState.Votes.Count);
            Assert.All(newState.ActivePlayers, p => Assert.Equal(PlayerStatus.Alive, p.Status));
        }
    }
}
