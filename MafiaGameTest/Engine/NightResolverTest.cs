using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using MafiaGame.Engine;
using MafiaGame.Engine.Roles;
using System.Net.Http.Headers;
using System.Linq;

namespace MafiaGameTest.Engine
{
    public class NightResolverTest : PlayerBase
    {
        private readonly NightResolver resolver = new NightResolver();

        [Theory]
        [InlineData("alice", "bob", "bob", "chris")]
        [InlineData("alice", "bob", "chris", "bob")]
        [InlineData("bob", "alice", "bob", "chris")]
        [InlineData("bob", "alice", "chris", "bob")]
        public void TwoBusDriversTargetOrderInvariant(string firstForB1, string secondForB1, string firstForB2, string secondForB2)
        {
            var action1 = new NightAction(bud, GetPlayer(firstForB1), GetPlayer(secondForB1));
            var action2 = new NightAction(buzz, GetPlayer(firstForB2), GetPlayer(secondForB2));

            bud.Role.OnResolveNightAction(state, action1, resolver);
            buzz.Role.OnResolveNightAction(state, action2, resolver);

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

        [Fact]
        public void BusDriverAndRoleBlocker()
        {
            var blockAction = new NightAction(brock, bud);
            var busAction = new NightAction(bud, alice, bob);

        }
    }
}
