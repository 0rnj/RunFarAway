using CodeBase.Gameplay.Enums;

namespace CodeBase.Gameplay.Controllers
{
    public interface IPlayerController
    {
        bool IsAlive { get; }
        
        void Tick(float deltaTime);
        void ProcessHit();
        void TryStrafe(StrafeDirection direction);
        void TryJump();
    }
}