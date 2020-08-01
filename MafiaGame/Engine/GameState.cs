using MafiaGame.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MafiaGame.Engine
{
    public sealed class GameState
    {
        public static GameState Blank { get; } = new GameState(
            Enumerable.Empty<PlayerSlot>(),
            Enumerable.Empty<Vote>(),
            Phase.Night);

        public IReadOnlyCollection<PlayerSlot> Players { get; }
        public IReadOnlyCollection<PlayerSlot> ActivePlayers { get; }
        public IReadOnlyCollection<Vote> Votes { get; }
        public Phase Phase { get; }

        private GameState(
            IEnumerable<PlayerSlot> players,
            IEnumerable<Vote> votes,
            Phase phase)
        {
            Players = players.ToReadOnlyCollection();
            ActivePlayers = players.Except(PlayerSlot.Host).ToReadOnlyCollection();
            Votes = votes.ToReadOnlyCollection();
            Phase = phase.AsDefinedOrThrow();
        }

        public GameState WithPlayers(IEnumerable<PlayerSlot> players)
        {
            return new GameState(
                Players.Union(players),
                Votes,
                Phase);
        }

        public GameState WithoutPlayer(PlayerSlot player)
        {
            return new GameState(
                Players.Except(player),
                Votes,
                Phase);
        }

        public GameState WithPlayerUpdate(PlayerSlot player, Func<PlayerSlot, PlayerSlot> updater)
        {
            var updatedPlayer = updater(player);
            return ReplacePlayer(player, updatedPlayer);
        }

        public GameState WithPlayerStatus(PlayerSlot player, PlayerStatus newStatus)
            => WithPlayerUpdate(player, p => new PlayerSlot(p.Owner, p.Role, newStatus));

        private GameState ReplacePlayer(PlayerSlot oldPlayer, PlayerSlot newPlayer)
        {
            IEnumerable<Vote> newVotes = Votes;
            IEnumerable<PlayerSlot> newPlayers = Players;

            // Update player roster
            newPlayers = newPlayers
                .Except(oldPlayer)
                .Append(newPlayer);

            // Update any voters
            newVotes = newVotes
                .Select(v => v.Voter == oldPlayer ? new Vote(newPlayer, v.Target) : v);

            // Update any vote targets
            newVotes = newVotes
                .Select(v => v.Target == oldPlayer ? new Vote(v.Voter, newPlayer) : v);

            return new GameState(newPlayers, newVotes, Phase);
        }

        public GameState WithVote(Vote vote)
        {
            return new GameState(
                Players,
                Votes.Append(vote),
                Phase);
        }

        public GameState WithoutVote(Vote vote)
        {
            return new GameState(
                Players,
                Votes.Except(vote),
                Phase);
        }

        public GameState WithoutVotes()
        {
            return new GameState(
                Players,
                Enumerable.Empty<Vote>(),
                Phase);
        }

        public GameState WithPhase(Phase newPhase)
        {
            return new GameState(
                Players,
                Votes,
                newPhase);
        }

        public PlayerSlot? GetPlayerFromOwner(Player owner)
        {
            return Players.FirstOrDefault(p => p.Owner == owner);
        }
    }
}
