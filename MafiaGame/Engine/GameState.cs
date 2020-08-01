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
            Enumerable.Empty<Vote>());

        public IReadOnlyCollection<PlayerSlot> Players { get; }
        public IReadOnlyCollection<Vote> Votes { get; }

        private GameState(
            IEnumerable<PlayerSlot> players,
            IEnumerable<Vote> votes)
        {
            Players = players.ToReadOnlyCollection();
            Votes = votes.ToReadOnlyCollection();
        }

        public GameState WithPlayers(IEnumerable<PlayerSlot> players)
        {
            return new GameState(
                Players.Union(players),
                Votes);
        }

        public GameState WithVote(Vote vote)
        {
            return new GameState(
                Players,
                Votes.Append(vote));
        }
    }
}
