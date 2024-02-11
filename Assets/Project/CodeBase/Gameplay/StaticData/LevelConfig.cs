using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Gameplay.StaticData
{
    [CreateAssetMenu(menuName = "StaticData/LevelConfig", fileName = "LevelConfig", order = 0)]
    public sealed class LevelConfig : ScriptableObject
    {
        [field: SerializeField] public int StartingEmptyBlocksCount { get; private set; }
        [field: SerializeField] public int StartingBlocksCount { get; private set; }
        [field: SerializeField] public Vector2Int BlockSize { get; private set; }
        [field: SerializeField] public float ObstacleOffsetX { get; private set; }
        [field: SerializeField] public float ObstaclePlacingStepZ { get; private set; }
        [field: SerializeField] public float NoObstacleChance { get; private set; }
        [field: SerializeField] public AssetReference ObstacleRef { get; private set; }
        [field: SerializeField] public AssetReference BlockRef { get; private set; }
    }
}