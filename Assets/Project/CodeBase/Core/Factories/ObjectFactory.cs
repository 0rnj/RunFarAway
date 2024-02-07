using System.Threading.Tasks;
using CodeBase.Core.Services.ResourcesLoading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;
using VContainer.Unity;

namespace CodeBase.Core.Factories
{
    public sealed class ObjectFactory : IObjectFactory
    {
        private readonly IResourcesService _resourcesService;
        private readonly IObjectResolver _objectResolver;

        public ObjectFactory(IResourcesService resourcesService, IObjectResolver objectResolver)
        {
            _resourcesService = resourcesService;
            _objectResolver = objectResolver;
        }

        public async Task<T> Create<T>(object owner, AssetReference assetReference)
            where T : Component
        {
            var instance = await _resourcesService.InstantiateAsync<T>(owner, assetReference);

            _objectResolver.InjectGameObject(instance.gameObject);

            return instance;
        }
    }
}