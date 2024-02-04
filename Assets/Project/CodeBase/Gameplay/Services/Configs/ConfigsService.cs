using System.Threading.Tasks;
using CodeBase.Core.Services;
using CodeBase.Core.Services.ResourcesLoading;
using CodeBase.Gameplay.StaticData;
using UnityEngine;

namespace CodeBase.Gameplay.Services.Configs
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