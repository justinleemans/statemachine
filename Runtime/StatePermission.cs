using System;
using System.Collections.Generic;

namespace JeeLee.StateMachine
{
    /// <summary>
    /// Represents permissions for state transitions in a state machine.
    /// </summary>
    /// <typeparam name="TState">The type of states in the state machine, must be an enumeration.</typeparam>
    public class StatePermission<TState>
        where TState : Enum
    {
        private HashSet<TState> _permittedStates = new HashSet<TState>();
        private Dictionary<TState, Func<bool>> _statePredicates = new Dictionary<TState, Func<bool>>();

        /// <summary>
        /// Permits a transition to the specified target state unconditionally.
        /// </summary>
        /// <param name="target">The target state to permit.</param>
        public void Permit(TState target)
        {
            Permit(target, () => true);
        }

        /// <summary>
        /// Permits a transition to the specified target state based on the provided predicate.
        /// </summary>
        /// <param name="target">The target state to permit.</param>
        /// <param name="predicate">A predicate function that determines whether the transition is permitted.</param>
        public void Permit(TState target, Func<bool> predicate)
        {
            _permittedStates.Add(target);
            _statePredicates[target] = predicate;
        }

        /// <summary>
        /// Checks whether a transition to the specified target state is permitted.
        /// </summary>
        /// <param name="target">The target state to check.</param>
        /// <returns><c>true</c> if the transition is permitted; otherwise, <c>false</c>.</returns>
        public bool CanTransition(TState target)
        {
            if (!_statePredicates.TryGetValue(target, out var predicate))
            {
                predicate = () => true;
            }
            
            return _permittedStates.Contains(target) && predicate();
        }
    }
}