using System;

namespace JeeLee.StateMachine
{
    /// <summary>
    /// Represents a behavior associated with a state in a state machine.
    /// </summary>
    /// <typeparam name="TState">The type of states in the state machine, must be an enumeration.</typeparam>
    public interface IStateBehaviour<out TState>
        where TState : Enum
    {
        /// <summary>
        /// Event triggered when the associated state is fired.
        /// </summary>
        event Action<TState> OnStateFired;

        /// <summary>
        /// Gets a value indicating whether the state behavior is currently active.
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Called when entering the associated state.
        /// </summary>
        void Enter();

        /// <summary>
        /// Called when exiting the associated state.
        /// </summary>
        void Exit();
    }
}