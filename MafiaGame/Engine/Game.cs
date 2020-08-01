using System;
using System.Collections.Generic;
using System.Text;

namespace MafiaGame.Engine
{
    public abstract class Game
    {
        public ActionPolicy ActionPolicy { get; }
        public GameState State { get; }

        protected Game(ActionPolicy actionPolicy, GameState initialState)
        {
            ActionPolicy = actionPolicy;
            State = initialState;
        }

        public abstract ActionResult Next(GameAction action);
    }
}
