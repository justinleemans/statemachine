using JeeLee.StateMachine;
using System;
using System.Collections.Generic;

namespace JeeLee.Statemachine
{
    public class StateMachine<TState>
        where TState : Enum
    {
        private readonly Dictionary<TState, IStateBehaviour<TState>> _stateBehaviours =
            new Dictionary<TState, IStateBehaviour<TState>>();
        
        private readonly Dictionary<TState, HashSet<TState>> _statePermissions =
            new Dictionary<TState, HashSet<TState>>();
        
        private Action<TState, TState> _transitionCallback;
        
        public TState State { get; private set; }

        public StateMachine(TState initialState = default) 
        {
            State = initialState;
        }

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

        public void SetPermission(TState source, TState target)
        {
            if (!_statePermissions.TryGetValue(source, out HashSet<TState> permissions))
            {
                permissions = new HashSet<TState>();
                _statePermissions.Add(source, permissions);
            }

            permissions.Add(target);
        }

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

        public void AddListener(Action<TState, TState> callback)
        {
            _transitionCallback += callback;
        }

        public void RemoveListener(Action<TState, TState> callback)
        {
            _transitionCallback -= callback;
        }

        private bool CheckStatePermission(TState target)
        {
            return _statePermissions[State].Contains(target);
        }

        private void TransitionState(TState source, TState target)
        {
            _stateBehaviours[source].Exit();
            State = target;
            _stateBehaviours[target].Enter();
        }
    }
}