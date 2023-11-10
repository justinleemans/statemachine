using System;

namespace JeeLee.StateMachine
{
    public interface IStateBehaviour<out TState>
        where TState : Enum
    {
        event Action<TState> OnStateFired;

        void Enter();
        void Exit();
    }
}
