using System;
using System.Collections.Generic;
using System.Text;

namespace MafiaGame.Engine
{
    public abstract class NightResult
    {
        public NightAction Action { get; }
        public bool IsSuccessful { get; }

        protected NightResult(NightAction action, bool isSuccessful)
        {
            Action = action;
            IsSuccessful = false;
        }

        public virtual string GetResultString()
        {
            if (!IsSuccessful)
                return "You were not successful.";

            return GetResultStringIfSuccessful();
        }

        protected abstract string GetResultStringIfSuccessful();
    }
}
