using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MafiaGame.Engine;
using Xunit;

namespace MafiaGameTest.Engine
{
    public class GameStateTest
    {
        private static readonly Player alicePlayer = new Player("Alice");
        private static readonly Player bobPlayer = new Player("Bob");
        private static readonly Player chrisPlayer = new Player("Chris");
        private static readonly Player denisePlayer = new Player("Denise");

        private static readonly PlayerSlot alice = new PlayerSlot(alicePlayer, Role.Town);
        private static readonly PlayerSlot bob = new PlayerSlot(bobPlayer, Role.Town);
        private static readonly PlayerSlot chris = new PlayerSlot(chrisPlayer, Role.Town);
        private static readonly PlayerSlot denise = new PlayerSlot(denisePlayer, Role.Town);

        [Theory]
        [MemberData(nameof(DistinctPlayersData))]
        public void PlayersAreAlwaysUnique(params PlayerSlot[][] playerSequences)
        {
            var state = GameState.Blank;
            foreach (var sequence in playerSequences)
            {
                state = state.WithPlayers(sequence);
            }
            var expectedPlayers = playerSequences.SelectMany(s => s).Distinct();
            Assert.True(expectedPlayers.SequenceEqual(state.Players));
        }

        public static IEnumerable<PlayerSlot[][]> DistinctPlayersData { get; } = new []
        {
            new []
            {
                new [] { alice },
                new [] { bob },
                new [] { chris },
                new [] { denise }
            },
            new []
            {
                new [] { alice, bob, chris, denise }
            },
            new []
            {
                new [] { alice },
                new [] { alice },
                new [] { bob }
            },
            new []
            {
                new [] { alice },
                new [] { alice, alice },
                new [] { bob }
            },
            new []
            {
                new [] { alice, alice, alice },
                new [] { alice, alice },
                new [] { bob, bob },
                new [] { chris, denise, alice, bob }
            },
            new []
            {
                new PlayerSlot[] { },
                new [] { alice, bob, chris, denise }
            }
        };
    }
}
