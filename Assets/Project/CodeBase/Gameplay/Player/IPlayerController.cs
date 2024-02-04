using Project.CodeBase.Gameplay.UI;

namespace Project.CodeBase.Gameplay.Player
{
    public interface IPlayerController : IController
    {
        bool IsAlive { get; }
        
        void Tick(float deltaTime);
        void ProcessHit();
        void TryStrafe(StrafeDirection direction);
        void TryJump();
    }
}