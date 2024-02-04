using System;
using System.Threading.Tasks;
using Project.CodeBase.Core.Factories;
using Project.CodeBase.Core.Services;
using Project.CodeBase.Core.Services.Configs;
using Project.CodeBase.Core.Services.Input;
using Project.CodeBase.Gameplay.Level;
using Project.CodeBase.Gameplay.Player;
using Project.CodeBase.Gameplay.UI;
using UnityEngine;
using VContainer.Unity;
using IInitializable = Project.CodeBase.Core.Services.IInitializable;

namespace Project.CodeBase.Gameplay
{
    public sealed class GameController : IGameController, IInitializable, ITickable, IDisposable
    {
        private readonly IConfigsService _configsService;
        private readonly ILevelController _levelController;
        private readonly IObjectFactory _objectFactory;
        private readonly IInputService _inputService;

        private int _currentBlockIndex;
        private bool _gameActive;
        private PlayerController _playerController;

        public event Action OnPlayerDied;

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

        async Task<bool> IInitializable.Initialize()
        {
            var visualsRef = _configsService.PlayerConfig.PlayerVisualsRef;
            
            var playerVisuals = await _objectFactory.CreateComponentGameObject(this, visualsRef);

            if (playerVisuals == null)
            {
                return false;
            }

            _levelController.OnObstacleHit += HandleObstacleHit;
            _inputService.OnStrafe += HandleStrafeRequested;
            _inputService.OnJump += HandleJumpRequested;

            return true;
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