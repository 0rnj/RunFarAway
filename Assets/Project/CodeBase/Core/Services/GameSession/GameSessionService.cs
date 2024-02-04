namespace Project.CodeBase.Core.Services.GameSession
{
    public sealed class GameSessionService : IGameSessionService
    {
        public int RunsCount { get; private set; }

        public void StartRun()
        {
            RunsCount++;
            
            // game controller logic here?
        }
    }
}