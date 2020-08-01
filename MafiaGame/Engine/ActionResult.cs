using MafiaGame.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace MafiaGame.Engine
{
    public sealed class ActionResult
    {
        public GameAction Action { get; }
        public ActionOutcome Outcome { get; }

        public ActionResult(GameAction action, ActionOutcome outcome)
        {
            Action = action;
            Outcome = outcome.AsDefinedOrThrow();
        }
    }

    public enum ActionOutcome
    {
        Success,
        Invalid,
        NotAllowed
    }
}
