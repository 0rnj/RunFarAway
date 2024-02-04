using System.Collections.Generic;

namespace Project.CodeBase.Gameplay
{
    public sealed class PlayerModel
    {
        private readonly List<BuffModel> _buffs = new();

        private int _hp;

        public bool IsAlive => _hp > 0;
        public IReadOnlyList<IBuffModel> Buffs => _buffs;

        public PlayerModel(int hp)
        {
            _hp = hp;
        }

        public void ProcessHit()
        {
            _hp--;
        }

        public void AddBuff(BuffModel buffModel)
        {
            _buffs.Add(buffModel);
        }

        public void TickBuffs(float deltaTime)
        {
            for (var i = 0; i < _buffs.Count; i++)
            {
                var buffModel = _buffs[i];

                buffModel.Tick(deltaTime);
            }
        }
    }
}