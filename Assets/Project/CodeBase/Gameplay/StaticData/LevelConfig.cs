using Project.CodeBase.Gameplay.Level;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Project.CodeBase.Gameplay.StaticData
{
    [CreateAssetMenu(menuName = "StaticData/LevelConfig", fileName = "LevelConfig", order = 0)]
    public sealed class LevelConfig : ScriptableObject
    {
        [field: SerializeField] public int StartingEmptyBlocksCount { get; private set; }
        [field: SerializeField] public Vector2Int BlockSize { get; private set; }
        [field: SerializeField] public float ObstacleOffsetX { get; private set; }
        [field: SerializeField] public float ObstaclePlacingStepZ { get; private set; }
        [field: SerializeField] public float SpaceForManeuverZ { get; private set; }
        [field: SerializeField] public Vector2Int BlockObstacleSizeZ { get; private set; }
        [field: SerializeField] public AssetReferenceT<Obstacle> ObstacleRef { get; private set; }
        [field: SerializeField] public AssetReferenceT<LevelBlock> BlockRef { get; private set; }
        
    }
}