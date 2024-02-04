using System.Threading.Tasks;
using Project.CodeBase.Core.Services.ResourcesLoading;
using Project.CodeBase.Gameplay.StaticData;
using UnityEngine;

namespace Project.CodeBase.Core.Services.Configs
{
    public sealed class ConfigsService : IConfigsService, IInitializable
    {
        private readonly IResourcesService _resourcesService;
        public GameConfig GameConfig { get; private set; }
        public PlayerConfig PlayerConfig { get; private set; }
        public LevelConfig LevelConfig { get; private set; }

        public ConfigsService(IResourcesService resourcesService)
        {
            _resourcesService = resourcesService;
        }

        public async Task<bool> Initialize()
        {
            GameConfig = await TryLoadConfig<GameConfig>();
            LevelConfig = await TryLoadConfig<LevelConfig>();
            PlayerConfig = await TryLoadConfig<PlayerConfig>();
            
            return LevelConfig != null && PlayerConfig != null;
        }

        private async Task<T> TryLoadConfig<T>() where T : ScriptableObject
        {
            var config = await _resourcesService.LoadAsync<T>(this, nameof(T));

            if (config == null)
            {
                Debug.LogError($"{nameof(T)} not loaded");
            }
            
            return config;
        }
    }
}