using System;
using Project.CodeBase.Gameplay.UI;
using UnityEngine;

namespace Project.CodeBase.Core.Services.Input
{
    public sealed class StandaloneInputService : MonoBehaviour, IInputService
    {
        public event Action OnJump;
        public event Action<StrafeDirection> OnStrafe;
        
        [SerializeField] private KeyCode _strafeLeftKey;
        [SerializeField] private KeyCode _strafeRightKey;
        [SerializeField] private KeyCode _jumpKey;

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(_strafeLeftKey))
            {
                OnStrafe?.Invoke(StrafeDirection.Left);
            }
            
            if (UnityEngine.Input.GetKeyDown(_strafeRightKey))
            {
                OnStrafe?.Invoke(StrafeDirection.Right);
            }

            if (UnityEngine.Input.GetKeyDown(_jumpKey))
            {
                OnJump?.Invoke();
            }
        }
    }
}