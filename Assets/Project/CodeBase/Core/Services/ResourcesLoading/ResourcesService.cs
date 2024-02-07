using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using Object = UnityEngine.Object;

namespace CodeBase.Core.Services.ResourcesLoading
{
    public sealed class ResourcesService : IResourcesService, IInitializableAsync
    {
        private const string ADDRESSABLES_LOCAL_LABEL = "LocalAsset";

        private readonly Dictionary<string, IResourceLocation> _resourcesLocations = new();
        private readonly Dictionary<string, AsyncOperationsContainer> _containers = new();
        private readonly Dictionary<object, string> _assetKeys = new();
        private readonly Dictionary<GameObject, string> _instanceKeys = new();

        int IInitializableAsync.InitOrder => 0;

        async Task<bool> IInitializableAsync.Initialize()
        {
            await Addressables.InitializeAsync().Task;

            var resourcesLocationsList = await Addressables.LoadResourceLocationsAsync(ADDRESSABLES_LOCAL_LABEL).Task;

            foreach (var resourcesLocation in resourcesLocationsList)
            {
                if (!IsValidResourceType(resourcesLocation.ResourceType))
                {
                    continue;
                }

                _resourcesLocations.Add(resourcesLocation.PrimaryKey, resourcesLocation);
            }

            return true;
        }

        public async Task<T> LoadAsync<T>(object owner, string primaryKey)
        {
            var resourceLocation = _resourcesLocations.GetValueOrDefault(primaryKey);

            var isComponent = typeof(Component).IsAssignableFrom(typeof(T));
            if (isComponent)
            {
                var aoh = Addressables.LoadAssetAsync<GameObject>(resourceLocation);

                GetAssetContainer(primaryKey).Add(owner, aoh);

                await aoh.Task;

                var result = aoh.Result;

                _assetKeys[result] = primaryKey;

                return result.GetComponent<T>();
            }
            else
            {
                var aoh = Addressables.LoadAssetAsync<T>(resourceLocation);

                GetAssetContainer(primaryKey).Add(owner, aoh);

                await aoh.Task;

                var result = aoh.Result;

                _assetKeys[result] = primaryKey;

                return result;
            }
        }

        public async Task<T> LoadAsync<T>(object owner, AssetReference reference)
        {
            var aoh = Addressables.LoadAssetAsync<T>(reference);

            GetAssetContainer(reference.AssetGUID).Add(owner, aoh);

            await aoh.Task;

            return aoh.Result;
        }

        public async Task<T> InstantiateAsync<T>(object owner,
            AssetReference reference,
            ResourceData data,
            InstantiationParameters parameters) where T : Object
        {
            AsyncOperationHandle<GameObject> aoh = default;

            try
            {
                aoh = Addressables.InstantiateAsync(reference, parameters);
            }
            catch (Exception e)
            {
                Debug.LogError($"[QA] Failed instantiate by reference: {e.Message}");
            }

            GetAssetContainer(reference.AssetGUID).Add(owner, aoh);

            await aoh.Task;

            ProcessData(data, aoh.Result);

            var result = aoh.Result;

            _instanceKeys[result] = reference.AssetGUID;

            if (typeof(T) == typeof(GameObject))
            {
                return result as T;
            }

            try
            {
                var component = result.GetComponent<T>();
                return component != null ? component : result as T;
            }
            catch (Exception)
            {
                return result as T;
            }
        }

        public void Release(object owner)
        {
            foreach (var container in _containers.Values)
            {
                container.Release(owner);
            }
        }

        public void Release<T>(object owner, T asset)
        {
            if (_assetKeys.Remove(asset, out var primaryKey))
            {
                GetAssetContainer(primaryKey).Release(owner, once: true);
            }
        }

        public void Release(object owner, AssetReference assetReference)
        {
            GetAssetContainer(assetReference.AssetGUID, false)?.Release(owner);
        }

        public void ReleaseScene(object owner, string primaryKey, SceneInstance sceneInstance)
        {
            if (string.IsNullOrEmpty(primaryKey))
            {
                Debug.LogError(
                    $"[{GetType().Name}] [{nameof(Release)}] Argument is null {nameof(primaryKey)} = {primaryKey}");
                return;
            }

            Addressables.UnloadSceneAsync(sceneInstance);

            GetAssetContainer(primaryKey, false)?.Release(owner);
        }

        public void ReleaseInstance(object owner, GameObject instance)
        {
            if (instance != null && _instanceKeys.TryGetValue(instance, out var primaryKey))
            {
                GetAssetContainer(primaryKey).Release(owner, once: true);

                Addressables.ReleaseInstance(instance);

                _instanceKeys.Remove(instance);
            }
            else
            {
                Debug.LogError($"PrimaryKey not found for {instance}, owner: {owner}");
            }
        }

        public void ReleaseNullOwners()
        {
            foreach (var asyncOperationsContainer in _containers.Values)
            {
                asyncOperationsContainer.Release(null);
            }
        }

        public void ReleaseAll()
        {
            foreach (var asyncOperationsContainer in _containers.Values)
            {
                asyncOperationsContainer.ReleaseAll();
            }
        }

        private bool IsValidResourceType(Type resourceType)
        {
            return resourceType.IsValueType == false &&
                   resourceType.ReflectedType == null &&
                   resourceType.BaseType != typeof(object) &&
                   resourceType.BaseType != typeof(UnityEventBase) &&
                   resourceType.BaseType != typeof(UnityEvent) &&
                   resourceType.BaseType?.BaseType != typeof(UnityEventBase) &&
                   resourceType.BaseType?.BaseType != typeof(UnityEvent);
        }

        private void ProcessData(ResourceData data, Object gameObject)
        {
            if (data.DontDestroy)
            {
                Object.DontDestroyOnLoad(gameObject);
            }

            if (!string.IsNullOrEmpty(data.Name))
            {
                gameObject.name = data.Name;
            }
        }

        private AsyncOperationsContainer GetAssetContainer(string primaryKey, bool createIfMissing = true)
        {
            if (_containers.TryGetValue(primaryKey, out var container))
            {
                return container;
            }

            if (createIfMissing == false)
            {
                return default;
            }

            container = new AsyncOperationsContainer(primaryKey);

            _containers[primaryKey] = container;

            return container;
        }
    }
}