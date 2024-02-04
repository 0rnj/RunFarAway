namespace Project.CodeBase.Core.StateMachine.Interfaces
{
    // TODO: move interface to proper place
    public interface IInstalledWith<in T>
    {
        void Install(T installation);
    }
}