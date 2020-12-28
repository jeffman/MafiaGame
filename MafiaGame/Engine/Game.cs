using System;
using System.Collections.Generic;
using System.Text;

namespace MafiaGame.Engine
{
    public class Game
    {
        public GameState State { get; }

        public Game(GameState initialState)
        {
            State = initialState;
        }
    }
}
