using CodeBase.Core.Services;
using CodeBase.Gameplay.StaticData;

namespace CodeBase.Gameplay.Services.Configs
{
    public interface IConfigsService : IService
    {
        GameConfig GameConfig { get; }
        PlayerConfig PlayerConfig { get; }
        LevelConfig LevelConfig { get; }
        BuffsConfig BuffsConfig { get; }
    }
}