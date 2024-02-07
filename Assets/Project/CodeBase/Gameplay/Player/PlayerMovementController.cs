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
            _timePassed = Time.time;
        }

        public void Move(float deltaTime)
        {
            var config = _configsService.PlayerConfig;
            var startingMoveSpeed = config.StartingMoveSpeed;
            var addedSpeed = (int)(_timePassed / config.MoveSpeedGainInterval) * config.MoveSpeedGain;
            var speedFactor = IsStrafing ? config.SpeedFactorWhileStrafing : 1f;
            var moveSpeed = (startingMoveSpeed + addedSpeed) * speedFactor;

            var visualsTransform = _playerVisuals.transform;
            var pos = visualsTransform.position;
            var x = GetPositionX();
            var y = pos.y;
            var z = pos.z + moveSpeed * deltaTime;
            
            visualsTransform.position = new Vector3(x, y, z);

            _timePassed += deltaTime;

            if (IsStrafing && _timePassed >= _endStrafeTime)
            {
                IsStrafing = false;
            }
        }

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

        private float GetPositionX()
        {
            var strafeTimeNormalized = Mathf.InverseLerp(_startStrafeTime, _endStrafeTime, _timePassed);
            var positionX = DOVirtual.EasedValue(
                _startStrafePositionX,
                _endStrafePositionX,
                strafeTimeNormalized,
                _configsService.PlayerConfig.StrafeEase);
            
            return positionX;
        }
    }
}