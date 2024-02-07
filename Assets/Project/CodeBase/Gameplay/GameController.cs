using System;
using System.Threading.Tasks;
using CodeBase.Core.Factories;
using CodeBase.Core.Services;
using CodeBase.Gameplay.Level;
using CodeBase.Gameplay.Player;
using CodeBase.Gameplay.Services.Configs;
using CodeBase.Gameplay.Services.Input;
using CodeBase.Gameplay.UI;
using UnityEngine;
using VContainer.Unity;

namespace CodeBase.Gameplay
{
    public sealed class GameController : IGameController, IInitializableAsync, ITickable, IDisposable
    {
        private readonly IConfigsService _configsService;
        private readonly ILevelController _levelController;
        private readonly IObjectFactory _objectFactory;
        private readonly IInputService _inputService;

        private int _currentBlockIndex;
        private bool _gameActive;
        private PlayerController _playerController;

        public event Action OnPlayerDied;

        int IInitializableAsync.InitOrder => 3;

        public GameController(
            IConfigsService configsService,
            ILevelController levelController,
            IObjectFactory objectFactory,
            IInputService inputService)
        {
            _configsService = configsService;
            _levelController = levelController;
            _objectFactory = objectFactory;
            _inputService = inputService;
        }

        Task<bool> IInitializableAsync.Initialize()
        {
            _levelController.OnObstacleHit += HandleObstacleHit;
            _inputService.OnStrafe += HandleStrafeRequested;
            _inputService.OnJump += HandleJumpRequested;

            return Task.FromResult(true);
        }

        void IDisposable.Dispose()
        {
            OnPlayerDied = null;
        }

        async Task IGameController.StartGame()
        {
            var emptyCount = _configsService.LevelConfig.StartingEmptyBlocksCount;

            _currentBlockIndex = -1;
            
            for (var i = 0; i < emptyCount; i++)
            {
                await _levelController.CreateEmptyBlock(_currentBlockIndex++);
            }

            _playerController = new PlayerController(_configsService, _objectFactory);
            
            await _playerController.Initialize();

            _gameActive = true;
        }

        void ITickable.Tick()
        {
            if (!_gameActive)
            {
                return;
            }

            var deltaTime = Time.deltaTime;

            _playerController.Tick(deltaTime);
        }

        private void HandleObstacleHit()
        {
            _playerController.ProcessHit();

            if (_playerController.IsAlive)
            {
                return;
            }

            _gameActive = false;
            
            OnPlayerDied?.Invoke();
        }

        private void HandleStrafeRequested(StrafeDirection direction)
        {
            _playerController.TryStrafe(direction);
        }

        private void HandleJumpRequested()
        {
            _playerController.TryJump();
        }
    }
}