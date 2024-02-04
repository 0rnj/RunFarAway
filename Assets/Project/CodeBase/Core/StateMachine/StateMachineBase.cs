using System;
using System.Collections.Generic;
using CodeBase.Core.StateMachine.Interfaces;

namespace CodeBase.Core.StateMachine
{
    public abstract class StateMachineBase
    {
        protected Dictionary<Type, IExitableState> States;
        protected IExitableState ActiveState;

        public void AddState<TState>(TState state) where TState : class, IExitableState
        {
            States ??= new Dictionary<Type, IExitableState>();

            States[typeof(TState)] = state;
        }
        
        public void Enter<TState>() where TState : class, IState
        {
            var state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IState<TPayload>
        {
            var state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            ActiveState?.Exit();
      
            var state = GetState<TState>();
            ActiveState = state;
      
            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState =>
            States[typeof(TState)] as TState;
    }

    public abstract class StateMachineBase<TKey, TState, TPayload> where TState : class, IState<TPayload>
    {
        protected Dictionary<TKey, TState> States;
        protected IExitableState ActiveState;
        
        public void AddState(TKey key, TState state)
        {
            States ??= new Dictionary<TKey, TState>();

            States[key] = state;
        }

        public void Enter(TKey key, TPayload payload)
        {
            var state = ChangeState(key);
            state.Enter(payload);
        }
    
        private TState ChangeState(TKey key)
        {
            ActiveState?.Exit();
      
            var state = GetState(key);
            ActiveState = state;
      
            return state;
        }
    
        private TState GetState(TKey key) => States[key];
    }
}