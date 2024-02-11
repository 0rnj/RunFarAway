namespace CodeBase.Gameplay.Models
{
    public interface IBuffModel
    {
        float Duration { get; }
        bool IsExpired { get; }
    }
}