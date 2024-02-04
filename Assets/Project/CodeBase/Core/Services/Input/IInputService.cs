using System;
using Project.CodeBase.Gameplay.UI;

namespace Project.CodeBase.Core.Services.Input
{
    public interface IInputService
    {
        event Action OnJump;
        event Action<StrafeDirection> OnStrafe;
    }
}