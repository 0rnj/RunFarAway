using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Gameplay.StaticData
{
    [CreateAssetMenu(menuName = "StaticData/GameConfig", fileName = "GameConfig", order = 0)]
    public sealed class GameConfig : ScriptableObject
    {
        [field: SerializeField] public string MenuSceneName { get; private set; }
        [field: SerializeField] public string GameSceneName { get; private set; }
        [field: SerializeField] public AssetReference StandaloneInputProviderRef { get; private set; }
        [field: SerializeField] public AssetReference CameraControllerRef { get; private set; }
    }
}