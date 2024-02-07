using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Core.Factories
{
    public interface IObjectFactory : IFactory
    {
        Task<T> Create<T>(object owner, AssetReference assetReference)
            where T : Component;
    }
}