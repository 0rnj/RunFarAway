using Project.CodeBase.Gameplay.StaticData;

namespace Project.CodeBase.Core.Services.Configs
{
    public interface IConfigsService : IService
    {
        GameConfig GameConfig { get; }
        PlayerConfig PlayerConfig { get; }
        LevelConfig LevelConfig { get; }
    }
}