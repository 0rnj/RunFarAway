using CodeBase.Gameplay.Services.Configs;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.Gameplay.Player
{
    public sealed class PlayerMovementController
    {
        private readonly IConfigsService _configsService;
        private readonly IPlayerModel _playerModel;
        private readonly PlayerVisuals _playerVisuals;

        private float _timePassed;
        private float _startStrafeTime;
        private float _startStrafePositionX;
        private float _endStrafePositionX;
        private float _endStrafeTime;

        private bool _isFlying;
        private Sequence _flyLandSequence;

        private bool _isStrafing;

        public PlayerMovementController(
            IConfigsService configsService,
            IPlayerModel playerModel,
            PlayerVisuals playerVisuals)
        {
            _configsService = configsService;
            _playerModel = playerModel;
            _playerVisuals = playerVisuals;
            _timePassed = Time.time;
        }

        public void Move(float deltaTime)
        {
            var config = _configsService.PlayerConfig;
            var startingMoveSpeed = config.StartingMoveSpeed;
            var addedSpeed = (int)(_timePassed / config.MoveSpeedGainInterval) * config.MoveSpeedGain;
            var speedMultiplier = 1f +
                                  _playerModel.MoveSpeedMultiplier +
                                  (_isStrafing ? config.SpeedMultiplierWhileStrafing : 0f);
            var moveSpeed = (startingMoveSpeed + addedSpeed) * speedMultiplier;
            moveSpeed = Mathf.Max(moveSpeed, config.MinMoveSpeed);

            var visualsTransform = _playerVisuals.transform;
            var pos = visualsTransform.position;
            var x = GetPositionX();
            var y = pos.y;
            var z = pos.z + moveSpeed * deltaTime;

            visualsTransform.position = new Vector3(x, y, z);

            _timePassed += deltaTime;

            if (_isStrafing && _timePassed >= _endStrafeTime)
            {
                _isStrafing = false;
            }

            var isFlying = _playerModel.IsFlying;
            if (isFlying != _isFlying)
            {
                SetFlying(isFlying);
            }
        }

        public void Strafe(float targetPositionX)
        {
            _isStrafing = true;
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

        private void SetFlying(bool isFlying)
        {
            _flyLandSequence?.Kill();

            _isFlying = isFlying;

            var target = isFlying ? _configsService.PlayerConfig.FlightHeight : 0f;

            _flyLandSequence = DOTween.Sequence()
                .Append(_playerVisuals.transform.DOLocalMoveY(target, 0.5f))
                .Play();
        }
    }
}