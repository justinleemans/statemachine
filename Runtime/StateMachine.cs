using JeeLee.StateMachine;
using System;
using System.Collections.Generic;

namespace JeeLee.Statemachine
{
    /// <summary>
    /// Represents a state machine.
    /// </summary>
    /// <typeparam name="TState">The type of states in the state machine, must be an enumeration.</typeparam>
    public class StateMachine<TState>
        where TState : Enum
    {
        private readonly Dictionary<TState, IStateBehaviour<TState>> _stateBehaviours =
            new Dictionary<TState, IStateBehaviour<TState>>();

        private readonly Dictionary<TState, StatePermission<TState>> _statePermissions =
            new Dictionary<TState, StatePermission<TState>>();

        private event Action<TState, TState> _transitionCallback;

        /// <summary>
        /// Gets or sets the current state of the state machine.
        /// </summary>
        public TState State { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StateMachine{TState}"/> class.
        /// </summary>
        /// <param name="initialState">The initial state of the state machine.</param>
        public StateMachine(TState initialState = default)
        {
            State = initialState;
        }

        /// <summary>
        /// Sets the behavior for a specific state.
        /// </summary>
        /// <param name="state">The state for which to set the behavior.</param>
        /// <param name="behaviour">The behavior to set.</param>
        public void SetStateBehaviour(TState state, IStateBehaviour<TState> behaviour)
        {
            if (_stateBehaviours.ContainsKey(state))
            {
                return;
            }

            behaviour.OnStateFired += Fire;
            _stateBehaviours.Add(state, behaviour);

            if (State.Equals(state))
            {
                behaviour.Enter();
            }
        }

        /// <summary>
        /// Sets a permission for transitioning from one state to another.
        /// </summary>
        /// <param name="source">The source state.</param>
        /// <param name="target">The target state.</param>
        public void SetPermission(TState source, TState target)
        {
            if (!_statePermissions.TryGetValue(source, out StatePermission<TState> permissions))
            {
                permissions = new StatePermission<TState>();
                _statePermissions.Add(source, permissions);
            }

            permissions.Permit(target);
        }

        /// <summary>
        /// Sets a permission for transitioning from one state to another based on a predicate.
        /// </summary>
        /// <param name="source">The source state.</param>
        /// <param name="target">The target state.</param>
        /// <param name="predicate">The predicate function to determine if the transition is allowed.</param>
        public void SetPermission(TState source, TState target, Func<bool> predicate)
        {
            if (!_statePermissions.TryGetValue(source, out StatePermission<TState> permissions))
            {
                permissions = new StatePermission<TState>();
                _statePermissions.Add(source, permissions);
            }

            permissions.Permit(target, predicate);
        }

        /// <summary>
        /// Fires a transition to the specified target state.
        /// </summary>
        /// <param name="target">The target state to transition to.</param>
        public void Fire(TState target)
        {
            if (!CheckStatePermission(target))
            {
                return;
            }

            TState source = State;
            TransitionState(source, target);
            _transitionCallback?.Invoke(source, target);
        }

        /// <summary>
        /// Adds a listener for state transitions.
        /// </summary>
        /// <param name="callback">The callback to invoke when a state transition occurs.</param>
        public void AddListener(Action<TState, TState> callback)
        {
            _transitionCallback += callback;
        }

        /// <summary>
        /// Removes a listener for state transitions.
        /// </summary>
        /// <param name="callback">The callback to remove.</param>
        public void RemoveListener(Action<TState, TState> callback)
        {
            _transitionCallback -= callback;
        }

        /// <summary>
        /// Checks if transitioning to the specified target state is permitted from the current state.
        /// </summary>
        /// <param name="target">The target state to transition to.</param>
        /// <returns><c>true</c> if transitioning to the target state is permitted; otherwise, <c>false</c>.</returns>
        private bool CheckStatePermission(TState target)
        {
            return _statePermissions[State].CanTransition(target);
        }

        /// <summary>
        /// Transitions from the source state to the target state.
        /// </summary>
        /// <param name="source">The source state.</param>
        /// <param name="target">The target state.</param>
        private void TransitionState(TState source, TState target)
        {
            _stateBehaviours[source].Exit();
            State = target;
            _stateBehaviours[target].Enter();
        }
    }
}