using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MafiaGame.Engine;
using MafiaGame.Engine.Roles;

namespace MafiaGameTest.Engine
{
    public class PlayerBase
    {
        protected readonly Player alice = new Player(Utility.Players.Alice, new TownRole());
        protected readonly Player bob = new Player(Utility.Players.Bob, new TownRole());
        protected readonly Player chris = new Player(Utility.Players.Chris, new TownRole());
        protected readonly Player denise = new Player(Utility.Players.Denise, new TownRole());
        protected readonly Player gabby = new Player(Utility.Players.Gabby, new GodfatherRole());
        protected readonly Player james = new Player(Utility.Players.James, new MafiaRole());
        protected readonly Player kelly = new Player(Utility.Players.Kelly, new MafiaRole());
        protected readonly Player larry = new Player(Utility.Players.Larry, new MafiaRole());
        protected readonly Player millhouse = new Player(Utility.Players.Millhouse, new MillerRole());
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
        }
    }
}
