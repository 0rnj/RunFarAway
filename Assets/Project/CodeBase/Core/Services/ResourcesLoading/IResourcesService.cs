using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace CodeBase.Core.Services.ResourcesLoading
{
    public interface IResourcesService
    {
        Task<T> LoadAsync<T>(object owner, string primaryKey);
        Task<T> LoadAsync<T>(object owner, AssetReference reference);

        Task<T> InstantiateAsync<T>(object owner,
            AssetReference reference,
            ResourceData data = default,
            InstantiationParameters parameters = default) where T : Object;

        void Release(object owner);
        void Release<T>(object owner, T asset);
        void Release(object owner, AssetReference assetReference);
        void ReleaseScene(object owner, string primaryKey, SceneInstance sceneInstance);
        void ReleaseInstance(object owner, GameObject instance);
        void ReleaseNullOwners();
        void ReleaseAll();
    }
}