using System.Diagnostics;

namespace MafiaGame.Engine.Roles
{
    public abstract class Role
    {
        public abstract Alignment Alignment { get; }
        public abstract Ability Ability { get; }
        public string Name { get; }
        public virtual int AllowedVotes => 1;

        protected Role(string name)
        {
            Name = name;
        }

        public virtual Alignment OnInvestigateAlignment(GameState state, Player owner, Player investigator)
        {
            return Alignment;
        }

        public virtual Ability OnInvestigateAbility(GameState state, Player owner, Player investigator)
        {
            return Ability;
        }

        /// <summary>
        /// Called when resolving dependencies for night actions.
        /// 
        /// May be called more than once per night phase, as some dependencies themselves may depend on other dependencies.
        /// </summary>
        public virtual void OnRegisterNightActionDependencies(GameState state, NightAction action, DependencyResolver resolver)
        {
        }

        public virtual void OnResolveNightPassiveAction(GameState state, Player owner, NightResolver resolver)
        {
        }

        public virtual void OnResolveNightAction(GameState state, NightAction action, NightResolver resolver)
        {
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
