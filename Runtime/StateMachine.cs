using JeeLee.StateMachine;
using System;
using System.Collections.Generic;

namespace JeeLee.Statemachine
{
    public class StateMachine<TState>
        where TState : Enum
    {
        private readonly Dictionary<TState, StateConfiguration<TState>> _stateConfigurations =
            new Dictionary<TState, StateConfiguration<TState>>();
        
        private readonly Dictionary<TState, HashSet<TState>> _statePermissions =
            new Dictionary<TState, HashSet<TState>>();
        
        private Action<TState, TState> _transitionCallback;
        
        public TState State { get; private set; }
        public bool IsInitialized { get; private set; }

        public StateMachine(TState initialState = default) 
        {
            State = initialState;
        }

        public void ConfigureState(TState state, StateBehaviour<TState> behaviour)
        {
            if (!_stateConfigurations.TryGetValue(state, out StateConfiguration<TState> configuration))
            {
                configuration = new StateConfiguration<TState>(this, state, behaviour);
                _stateConfigurations.Add(state, configuration);
            }

            if (IsInitialized)
                return;

            IsInitialized = true;
            configuration.Enter();
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
                return;

            TState source = State;
            _transitionCallback?.Invoke(source, target);
            TransitionState(source, target);
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
            _stateConfigurations[source].Exit();
            State = target;
            _stateConfigurations[target].Enter();
        }
    }
}
