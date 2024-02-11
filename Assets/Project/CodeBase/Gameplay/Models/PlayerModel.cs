using System.Collections.Generic;
using System.Linq;
using CodeBase.Gameplay.Enums;
using CodeBase.Gameplay.StaticData;

namespace CodeBase.Gameplay.Models
{
    public sealed class PlayerModel : IPlayerModel
    {
        private readonly List<BuffModel> _buffs = new();

        private int _hp;
        private readonly BuffsConfig _buffsConfig;

        public bool IsAlive => _hp > 0;
        public IReadOnlyList<IBuffModel> Buffs => _buffs;
        public float MoveSpeedMultiplier { get; private set; }
        public bool IsFlying { get; private set; }

        public PlayerModel(int hp, BuffsConfig buffsConfig)
        {
            _hp = hp;
            _buffsConfig = buffsConfig;
        }

        public void ProcessHit()
        {
            _hp--;
        }

        public void AddBuff(BuffConfig buffConfig)
        {
            AddSpeedEffect(buffConfig.BuffType);

            _buffs.Add(new BuffModel(buffConfig.BuffType, buffConfig.Duration));

            UpdateIsFlyingState();
        }

        public void TickBuffs(float deltaTime)
        {
            for (var i = _buffs.Count - 1; i >= 0; i--)
            {
                var buffModel = _buffs[i];

                buffModel.Tick(deltaTime);

                if (!buffModel.IsExpired)
                {
                    continue;
                }

                _buffs.RemoveAt(i);

                RemoveSpeedEffect(buffModel.BuffType);
                UpdateIsFlyingState();
            }
        }

        private void AddSpeedEffect(BuffType buffType)
        {
            var buffPower = GetBuffPower(buffType);

            MoveSpeedMultiplier += buffPower;
        }

        private void RemoveSpeedEffect(BuffType buffType)
        {
            var buffPower = GetBuffPower(buffType);

            MoveSpeedMultiplier -= buffPower;
        }

        private float GetBuffPower(BuffType buffType)
        {
            var buffPower = buffType switch
            {
                BuffType.Slow => _buffsConfig.SlowPower,
                BuffType.SpeedUp => _buffsConfig.SpeedUpPower,
                _ => 0f
            };
            return buffPower;
        }

        private void UpdateIsFlyingState()
        {
            IsFlying = _buffs.Count > 0 && _buffs.Any(model => model.BuffType is BuffType.Flight);
        }
    }
}