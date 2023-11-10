using System;

namespace JeeLee.StateMachine
{
    public abstract class StateBehaviour<TState> : IStateBehaviour<TState>
        where TState : Enum
    {
        #region IStateBehaviour Members

        public event Action<TState> OnStateFired;

        public void Enter()
        {
            OnEnter();
        }

        public void Exit()
        {
            OnExit();
        }

        #endregion

        protected void Fire(TState state)
        {
            OnStateFired?.Invoke(state);
        }

        protected abstract void OnEnter();
        protected abstract void OnExit();
    }
}
