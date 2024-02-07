using System.Collections.Generic;
using CodeBase.Core.Factories;
using CodeBase.Gameplay.Common;
using CodeBase.Gameplay.StaticData;
using UnityEngine;
using VContainer;

namespace CodeBase.Gameplay.Level
{
    public sealed class LevelBlock : MonoBehaviour
    {
        private readonly List<Obstacle> _obstacles = new();

        [SerializeField] private Transform _scaledContent;
        [SerializeField] private CollisionEventsProvider _collisionEventsProvider;

        private LevelConfig _config;

        public void Initialize(LevelConfig config)
        {
            _config = config;
            _scaledContent.localScale = Vector3.one + Vector3.forward * config.BlockSize.y;
        }
        
        // TODO: fill & release
        public void AddObstacle(Obstacle obstacle, int x, int z)
        {
            var sizeX = _config.BlockSize.x;
            var middleIndexX = sizeX / 2;
            var offsetX = _config.ObstacleOffsetX;
            var resultOffsetX = x - middleIndexX * offsetX;
            var resultOffsetZ = z * _config.ObstaclePlacingStepZ;

            obstacle.transform.localPosition = new Vector3(resultOffsetX, 0f, resultOffsetZ);
            
            _obstacles.Add(obstacle);
        }
    }
}