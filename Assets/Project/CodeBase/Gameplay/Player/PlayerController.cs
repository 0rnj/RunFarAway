using System;
using System.Threading.Tasks;
using CodeBase.Core.Factories;
using CodeBase.Gameplay.Services.Configs;
using CodeBase.Gameplay.UI;

namespace CodeBase.Gameplay.Player
{
    public sealed class PlayerController : IPlayerController
    {
        private readonly IConfigsService _configsService;
        private readonly IObjectFactory _objectFactory;

        private PlayerModel _model;
        private PlayerVisuals _visuals;
        private PlayerMovementController _movementController;
        private CameraController _cameraController;
        private int _currentLineIndex;

        public int InitOrder => 2;
        
        public bool IsAlive => _model.IsAlive;

        public PlayerController(IConfigsService configsService, IObjectFactory objectFactory)
        {
            _configsService = configsService;
            _objectFactory = objectFactory;
        }

        public async Task<bool> Initialize()
        {
            var playerConfig = _configsService.PlayerConfig;
            var visualsRef = playerConfig.PlayerVisualsRef;
            var cameraRef = _configsService.GameConfig.CameraControllerRef;

            _model = new PlayerModel(playerConfig.Hp);

            _visuals = await _objectFactory.Create<PlayerVisuals>(this, visualsRef);
            _movementController = new PlayerMovementController(_configsService, _visuals);

            _cameraController = await _objectFactory.Create<CameraController>(this, cameraRef);
            _cameraController.SetFollowTarget(_visuals.transform);

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
            var targetPositionX = (targetLineIndex - middleIndexX) * offsetX;

            _movementController.Strafe(targetPositionX);

            _currentLineIndex = targetLineIndex;
        }
        
        public void TryJump()
        {
            //
        }
    }
}