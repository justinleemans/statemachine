using System;

namespace JeeLee.UniStateMachine
{
    /// <summary>
    /// Represents a base class for state behaviors in a state machine.
    /// </summary>
    /// <typeparam name="TState">The type of states in the state machine, must be an enumeration.</typeparam>
    public abstract class StateBehaviour<TState> : IStateBehaviour<TState>
        where TState : Enum
    {
        #region IStateBehaviour Members

        /// <summary>
        /// Event triggered when the associated state is fired.
        /// </summary>
        public event Action<TState> OnStateFired;

        /// <summary>
        /// Gets a value indicating whether the state behavior is currently active.
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Called when entering the associated state.
        /// </summary>
        public void Enter()
        {
            OnEnter();

            IsActive = true;
        }

        /// <summary>
        /// Called when exiting the associated state.
        /// </summary>
        public void Exit()
        {
            IsActive = false;

            OnExit();
        }

        #endregion

        /// <summary>
        /// Fires the event indicating that the associated state has been fired.
        /// </summary>
        /// <param name="state">The state that has been fired.</param>
        protected void Fire(TState state)
        {
            OnStateFired?.Invoke(state);
        }

        /// <summary>
        /// A method to be overridden by derived classes, called when entering the associated state.
        /// </summary>
        protected abstract void OnEnter();

        /// <summary>
        /// A method to be overridden by derived classes, called when exiting the associated state.
        /// </summary>
        protected abstract void OnExit();
    }
}