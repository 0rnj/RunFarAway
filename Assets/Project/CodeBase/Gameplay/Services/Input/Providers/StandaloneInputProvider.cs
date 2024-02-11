using System;
using CodeBase.Gameplay.Enums;
using CodeBase.Gameplay.UI;
using UnityEngine;

namespace CodeBase.Gameplay.Services.Input.Providers
{
    public sealed class StandaloneInputProvider : MonoBehaviour, IInputProvider
    {
        [SerializeField] private KeyCode _strafeLeftKey;
        [SerializeField] private KeyCode _strafeRightKey;
        [SerializeField] private KeyCode _jumpKey;

        public event Action OnJump;
        public event Action<StrafeDirection> OnStrafe;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void SetActive(bool active)
        {
            enabled = active;
        }

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