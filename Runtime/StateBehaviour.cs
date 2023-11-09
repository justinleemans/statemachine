using JeeLee.Statemachine;
using System;

namespace JeeLee.StateMachine
{
    public abstract class StateBehaviour<TState>
        where TState : Enum
    {
        protected StateMachine<TState> StateMachine { get; private set; }

        public void SetSateMachine(StateMachine<TState> stateMachine)
        {
            StateMachine = stateMachine;
        }

        public abstract void OnEnter();
        public abstract void OnExit();
    }
}
