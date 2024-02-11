using System;
using System.Threading.Tasks;
using CodeBase.Core.Factories;
using CodeBase.Core.Services;
using CodeBase.Gameplay.Enums;
using CodeBase.Gameplay.Services.Configs;
using CodeBase.Gameplay.Services.Input.Providers;
using CodeBase.Gameplay.Services.UI;
using CodeBase.Gameplay.UI;

namespace CodeBase.Gameplay.Services.Input
{
    public sealed class InputService : IInputService, IInitializableAsync, IDisposable
    {
        private readonly IConfigsService _configsService;
        private readonly IUIService _uiService;
        private readonly IObjectFactory _objectFactory;

        private IInputProvider _inputProvider;

        public event Action OnJump;
        public event Action<StrafeDirection> OnStrafe;

        int IInitializableAsync.InitOrder => 2;

        public InputService(
            IConfigsService configsService,
            IUIService uiService,
            IObjectFactory objectFactory)
        {
            _configsService = configsService;
            _uiService = uiService;
            _objectFactory = objectFactory;
        }

        async Task<bool> IInitializableAsync.Initialize()
        {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            _inputProvider = await _uiService.Show<MobileInputProvider>();
#else
            var inputProviderRef = _configsService.GameConfig.StandaloneInputProviderRef;
            _inputProvider = await _objectFactory.Create<StandaloneInputProvider>(this, inputProviderRef);
#endif

            if (_inputProvider == null)
            {
                return false;
            }

            _inputProvider.OnJump += HandleJump;
            _inputProvider.OnStrafe += HandleStrafe;

            return true;
        }

        void IDisposable.Dispose()
        {
            if (_inputProvider == null)
            {
                return;
            }
            
            _inputProvider.OnJump -= HandleJump;
            _inputProvider.OnStrafe -= HandleStrafe;
        }

        private void HandleJump()
        {
            OnJump?.Invoke();
        }

        private void HandleStrafe(StrafeDirection direction)
        {
            OnStrafe?.Invoke(direction);
        }
    }
}