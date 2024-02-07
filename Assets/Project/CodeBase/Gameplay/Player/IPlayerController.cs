﻿using CodeBase.Gameplay.UI;

namespace CodeBase.Gameplay.Player
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