using System;
using CodeBase.Gameplay.UI;

namespace CodeBase.Gameplay.Services.Input.Providers
{
    public interface IInputProvider
    {
        public event Action OnJump;
        public event Action<StrafeDirection> OnStrafe;

        void SetActive(bool active);
    }
}