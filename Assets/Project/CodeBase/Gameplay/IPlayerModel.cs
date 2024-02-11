using System.Collections.Generic;

namespace CodeBase.Gameplay
{
    public interface IPlayerModel
    {
        bool IsAlive { get; }
        IReadOnlyList<IBuffModel> Buffs { get; }
        float MoveSpeedMultiplier { get; }
        bool IsFlying { get; }
    }
}