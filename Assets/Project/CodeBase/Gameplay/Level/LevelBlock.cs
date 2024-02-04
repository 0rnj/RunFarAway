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
        private IObjectFactory _objectFactory;

        [Inject]
        private void Construct(IObjectFactory objectFactory, LevelConfig levelConfig)
        {
            _objectFactory = objectFactory;
            _config = levelConfig;
        }

        public void Initialize(LevelConfig config)
        {
            _config = config;
        }
        
        // TODO: fill & release
        public void AddObstacle(Obstacle obstacle, int x, int y)
        {
            var sizeX = _config.BlockSize.x;
            var middleIndexX = sizeX / 2;
            var offsetX = _config.ObstacleOffsetX;
            var resultOffsetX = x - middleIndexX * offsetX;
            var resultOffsetY = y * _config.ObstaclePlacingStepZ;

            obstacle.transform.localPosition = new Vector2(resultOffsetX, resultOffsetY);
            
            _obstacles.Add(obstacle);
        }
    }
}