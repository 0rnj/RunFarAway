using System;
using CodeBase.Core.Services;
using CodeBase.Gameplay.Enums;
using CodeBase.Gameplay.UI;

namespace CodeBase.Gameplay.Services.Input
{
    public interface IInputService : IService
    {
        event Action OnJump;
        event Action<StrafeDirection> OnStrafe;
    }
}