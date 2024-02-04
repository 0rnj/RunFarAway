using Project.CodeBase.Core.StateMachine.Interfaces;

namespace Project.CodeBase.Core.StateMachine
{
    public abstract class StateBase : IState, IInstalledWith<StateMachineBase>
    {
        protected StateMachineBase StateMachine;

        public abstract void Exit();
        public abstract void Enter();

        void IInstalledWith<StateMachineBase>.Install(StateMachineBase stateMachine)
        {
            StateMachine = stateMachine;
        }
    }
    
    public abstract class StateBase<T> : IState<T>, IInstalledWith<StateMachineBase>
    {
        protected StateMachineBase StateMachine;

        void IInstalledWith<StateMachineBase>.Install(StateMachineBase stateMachine)
        {
            StateMachine = stateMachine;
        }

        public abstract void Enter(T payload);
        public abstract void Exit();
    }
}