using System;
using System.Threading.Tasks;
using CodeBase.Core.Factories;
using CodeBase.Core.Services;
using CodeBase.Gameplay.Services.Configs;
using CodeBase.Gameplay.UI;

namespace CodeBase.Gameplay.Player
{
    public sealed class PlayerController : IInitializable, IPlayerController
    {
        private readonly IConfigsService _configsService;
        private readonly IObjectFactory _objectFactory;

        private PlayerVisuals _visuals;
        private PlayerModel _model;
        private PlayerMovementController _movementController;
        private int _currentLineIndex;

        public bool IsAlive => _model.IsAlive;

        public PlayerController(IConfigsService configsService, IObjectFactory objectFactory)
        {
            _configsService = configsService;
            _objectFactory = objectFactory;
        }

        async Task<bool> IInitializable.Initialize()
        {
            var playerConfig = _configsService.PlayerConfig;
            var visualsRef = playerConfig.PlayerVisualsRef;

            _model = new PlayerModel(playerConfig.Hp);

            _visuals = await _objectFactory.CreateComponentGameObject(this, visualsRef);

            _currentLineIndex = _configsService.LevelConfig.BlockSize.x / 2;

            return _visuals != null;
        }

        public void Tick(float deltaTime)
        {
            _movementController.Move(deltaTime);
        }

        public void ProcessHit()
        {
            _model.ProcessHit();
        }

        public void TryStrafe(StrafeDirection direction)
        {
            // if (!CanStrafe(direction, out var targetLineIndex))
            // {
            //     return;
            // }

            var targetLineIndex = direction switch
            {
                StrafeDirection.Left => _currentLineIndex - 1,
                StrafeDirection.Right => _currentLineIndex + 1,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };

            if (_movementController.IsStrafing ||
                targetLineIndex < 0 ||
                targetLineIndex >= _configsService.LevelConfig.BlockSize.x)
            {
                return;
            }

            var config = _configsService.LevelConfig;
            var sizeX = config.BlockSize.x;
            var middleIndexX = sizeX / 2;
            var offsetX = config.ObstacleOffsetX;
            var targetPositionX = targetLineIndex - middleIndexX * offsetX;

            _movementController.Strafe(targetPositionX);

            _currentLineIndex = targetLineIndex;
        }
        
        public void TryJump()
        {
            //
        }

        // private bool CanStrafe(StrafeDirection direction, out int targetLineIndex)
        // {
        //     targetLineIndex = direction switch
        //     {
        //         StrafeDirection.Left => _currentLineIndex - 1,
        //         StrafeDirection.Right => _currentLineIndex + 1,
        //         _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        //     };
        //
        //     return !_movementController.IsStrafing &&
        //            targetLineIndex > 0 &&
        //            targetLineIndex < _configsService.LevelConfig.BlockSize.x;
        // }
    }
}