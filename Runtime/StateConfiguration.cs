using JeeLee.Statemachine;
using System;

namespace JeeLee.StateMachine
{
    public sealed class StateConfiguration<TState>
        where TState : Enum
    {
        private readonly StateMachine<TState> _stateMachine;
        private readonly TState _state;
        private readonly StateBehaviour<TState> _stateBehaviour;
        
        public StateConfiguration(StateMachine<TState> stateMachine, TState state, StateBehaviour<TState> behaviour)
        {
            behaviour.SetSateMachine(stateMachine);

            _stateMachine = stateMachine;
            _state = state;
            _stateBehaviour = behaviour;
        }

        public void Enter()
        {
            _stateBehaviour.OnEnter();
        }

        public void Exit()
        {
            _stateBehaviour.OnExit();
        }
    }
}
