namespace Project.CodeBase.Gameplay
{
    public interface IBuffModel
    {
        float Duration { get; }
        bool IsExpired { get; }
    }
}