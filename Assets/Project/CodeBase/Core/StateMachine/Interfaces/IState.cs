namespace CodeBase.Core.StateMachine.Interfaces
{
    public interface IState : IExitableState
    {
        void Enter();
    }

    public interface IState<in T> : IExitableState
    {
        void Enter(T value);
    }
}