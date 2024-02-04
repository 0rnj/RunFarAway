using System;
using CodeBase.Gameplay.UI;

namespace CodeBase.Gameplay.Services.Input
{
    public interface IInputService
    {
        event Action OnJump;
        event Action<StrafeDirection> OnStrafe;
    }
}