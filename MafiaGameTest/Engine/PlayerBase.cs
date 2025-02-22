﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MafiaGame.Engine;
using MafiaGame.Engine.Roles;

namespace MafiaGameTest.Engine
{
    public class PlayerBase
    {
        protected readonly Player alice = new Player(Utility.People.Alice, new TownRole());
        protected readonly Player bob = new Player(Utility.People.Bob, new TownRole());
        protected readonly Player chris = new Player(Utility.People.Chris, new TownRole());
        protected readonly Player denise = new Player(Utility.People.Denise, new TownRole());
        protected readonly Player gabby = new Player(Utility.People.Gabby, new GodfatherRole());
        protected readonly Player james = new Player(Utility.People.James, new MafiaRole());
        protected readonly Player kelly = new Player(Utility.People.Kelly, new MafiaRole());
        protected readonly Player larry = new Player(Utility.People.Larry, new MafiaRole());
        protected readonly Player millhouse = new Player(Utility.People.Millhouse, new MillerRole());
        protected readonly Player bud = new Player(Utility.People.Bud, new BusDriverRole(Alignment.Town));
        protected readonly Player buzz = new Player(Utility.People.Buzz, new BusDriverRole(Alignment.Town));
        protected readonly Player brock = new Player(Utility.People.Brock, new BlockerRole(Alignment.Mafia));
        protected readonly GameState state;

        public IEnumerable<object[]> Mafia => state.Players.Where(p => p.Role.Alignment == Alignment.Mafia).AsMemberData();
        public IEnumerable<object[]> Town => state.Players.Where(p => p.Role.Alignment == Alignment.Town).AsMemberData();
        public IEnumerable<object[]> All => state.Players.AsMemberData();

        protected PlayerBase()
        {
            state = new GameState();
            state.Players.Add(alice);
            state.Players.Add(bob);
            state.Players.Add(chris);
            state.Players.Add(denise);
            state.Players.Add(gabby);
            state.Players.Add(james);
            state.Players.Add(kelly);
            state.Players.Add(larry);
            state.Players.Add(millhouse);
            state.Players.Add(bud);
            state.Players.Add(buzz);
            state.Players.Add(brock);
        }

        protected Player GetPlayer(string name)
        {
            return state.Players.First(p => p.Owner.Name.ToLowerInvariant() == name.ToLowerInvariant());
        }
    }
}
