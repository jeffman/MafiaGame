using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using MafiaGame.Engine;
using MafiaGame.Engine.Roles;
using MafiaGame.Engine.Actions;
using System.Net.Http.Headers;
using System.Linq;

namespace MafiaGameTest.Engine
{
    public class NightResolverTest : PlayerBase
    {
        private readonly NightResolver resolver = new NightResolver();
        private readonly Player busDriver1 = new Player(new Person("Bus driver 1"), new BusDriverRole(Alignment.Town));
        private readonly Player busDriver2 = new Player(new Person("Bus driver 2"), new BusDriverRole(Alignment.Town));

        public NightResolverTest()
        {
            state.Players.Add(busDriver1);
            state.Players.Add(busDriver2);
        }

        [Theory]
        [InlineData("alice", "bob", "bob", "chris")]
        [InlineData("alice", "bob", "chris", "bob")]
        [InlineData("bob", "alice", "bob", "chris")]
        [InlineData("bob", "alice", "chris", "bob")]
        public void TwoBusDriversTargetOrderInvariant(string firstForB1, string secondForB1, string firstForB2, string secondForB2)
        {
            var action1 = new NightAction(busDriver1, new[] { GetPlayer(firstForB1), GetPlayer(secondForB1) });
            var action2 = new NightAction(busDriver2, new[] { GetPlayer(firstForB2), GetPlayer(secondForB2) });

            busDriver1.Role.OnResolveNightAction(state, action1, resolver);
            busDriver2.Role.OnResolveNightAction(state, action2, resolver);

            Assert.Equal(chris, resolver.GetActualTarget(alice));
            Assert.Equal(alice, resolver.GetActualTarget(bob));
            Assert.Equal(alice, resolver.GetActualTarget(chris));
        }

        [Fact]
        public void RedirectOverridesExistingRedirect()
        {
            resolver.RedirectTarget(alice, bob);
            resolver.RedirectTarget(alice, chris);
            Assert.Equal(chris, resolver.GetActualTarget(alice));
        }

        private Player GetPlayer(string name)
        {
            return state.Players.First(p => p.Owner.Name.ToLowerInvariant() == name.ToLowerInvariant());
        }
    }
}
