using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Project.CodeBase.Core.Factories
{
    public interface IObjectFactory : IFactory
    {
        Task<GameObject> CreateGameObject(object owner, AssetReferenceT<GameObject> assetReference);

        Task<T> CreateComponentGameObject<T>(object owner, AssetReferenceT<T> assetReference)
            where T : Component;
    }
}