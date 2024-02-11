using CodeBase.Gameplay.Enums;

namespace CodeBase.Gameplay.Models
{
    public class BuffModel : IBuffModel
    {
        public BuffType BuffType { get; }
        public float Duration { get; private set; }
        public bool IsExpired => Duration <= 0f;

        public BuffModel(BuffType buffType, float duration)
        {
            BuffType = buffType;
            Duration = duration;
        }

        public void Tick(float deltaTime)
        {
            Duration -= deltaTime;
        }
    }
}