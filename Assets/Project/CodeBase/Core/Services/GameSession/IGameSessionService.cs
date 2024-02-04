namespace Project.CodeBase.Core.Services.GameSession
{
    public interface IGameSessionService : IService
    {
        void StartRun();
        int RunsCount { get; }
    }
}