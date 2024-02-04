using CodeBase.Gameplay.Services.Configs;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.Gameplay.Player
{
    public sealed class PlayerMovementController
    {
        private readonly IConfigsService _configsService;
        private readonly PlayerVisuals _playerVisuals;

        private float _timePassed;
        private float _startStrafeTime;
        private float _startStrafePositionX;
        private float _endStrafePositionX;
        private float _endStrafeTime;
        
        public bool IsStrafing { get; private set; }

        public PlayerMovementController(IConfigsService configsService, PlayerVisuals playerVisuals)
        {
            _configsService = configsService;
            _playerVisuals = playerVisuals;
        }

        public void Move(float deltaTime)
        {
            var config = _configsService.PlayerConfig;
            var startingMoveSpeed = config.StartingMoveSpeed;
            var addedSpeed = (int)(_timePassed / config.MoveSpeedGainInterval) * config.MoveSpeedGain;
            var moveSpeed = startingMoveSpeed + addedSpeed;
            var direction = GetDirection();

            _playerVisuals.transform.position += direction * moveSpeed * deltaTime;

            _timePassed += deltaTime;
        }

        // public bool CanStrafe(StrafeDirection direction)
        // {
        //     return !IsStrafing && _configsService.LevelConfig.BlockSize.x
        // }

        public void Strafe(float targetPositionX)
        {
            if (IsStrafing)
            {
                return;
            }
            
            IsStrafing = true;
            _startStrafePositionX = _playerVisuals.transform.position.x;
            _endStrafePositionX = targetPositionX;
            _startStrafeTime = Time.time;
            _endStrafeTime = _startStrafeTime + _configsService.PlayerConfig.StrafeDuration;
        }

        private Vector3 GetDirection()
        {
            if (!IsStrafing)
            {
                return Vector3.forward;
            }

            var strafeTimeNormalized = Mathf.InverseLerp(_startStrafeTime, _endStrafeTime, _timePassed);
            var positionX = DOVirtual.EasedValue(
                _startStrafePositionX,
                _endStrafePositionX,
                strafeTimeNormalized,
                _configsService.PlayerConfig.StrafeEase);
            var direction = (Vector3.forward + Vector3.right * positionX).normalized;

            return direction;
        }
    }
}