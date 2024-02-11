using System;
using System.Collections.Generic;
using CodeBase.Gameplay.Common;
using CodeBase.Gameplay.StaticData;
using UnityEngine;

namespace CodeBase.Gameplay.Level
{
    public sealed class LevelBlock : MonoBehaviour
    {
        private readonly List<Obstacle> _obstacles = new();

        [SerializeField] private Transform _scaledContent;
        [SerializeField] private CollisionEventsProvider _collisionEventsProvider;

        private LevelConfig _config;

        public event Action<LevelBlock> OnEnter;

        public bool IsEmpty => _obstacles.Count == 0;

        public void Initialize(LevelConfig config, bool isEmpty)
        {
            _config = config;
            _scaledContent.localScale = Vector3.one + Vector3.forward * config.BlockSize.y * config.ObstaclePlacingStepZ;
            
            // var sizeZ = isEmpty
            //     ? Vector3.forward * config.EmptyBlockSize
            //     : Vector3.forward * config.BlockSize.y * config.ObstaclePlacingStepZ;
            //
            // _scaledContent.localScale = Vector3.one + sizeZ;
        }
        
        // TODO: fill & release
        public void AddObstacle(Obstacle obstacle, int x, int z)
        {
            var sizeX = _config.BlockSize.x;
            var middleIndexX = sizeX / 2;
            var offsetX = _config.ObstacleOffsetX;
            var resultOffsetX = (x - middleIndexX) * offsetX;
            var resultOffsetZ = z * _config.ObstaclePlacingStepZ;
            var obstacleTransform = obstacle.transform;

            obstacleTransform.SetParent(transform);
            obstacleTransform.localPosition = new Vector3(resultOffsetX, 0f, resultOffsetZ);
            
            _obstacles.Add(obstacle);
        }

        private void OnEnable()
        {
            _collisionEventsProvider.OnCollide += HandleBlockEnter;
        }

        private void OnDisable()
        {
            _collisionEventsProvider.OnCollide -= HandleBlockEnter;
        }

        private void HandleBlockEnter()
        {
            OnEnter?.Invoke(this);        
        }
    }
}