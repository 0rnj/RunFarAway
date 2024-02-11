using System;
using System.Collections.Generic;
using CodeBase.Gameplay.StaticData;
using UnityEngine;

namespace CodeBase.Gameplay.Scene.Level
{
    public sealed class LevelBlock : MonoBehaviour
    {
        private readonly List<Obstacle> _obstacles = new();

        [SerializeField] private Transform _scaledContent;
        [SerializeField] private CollisionEventsProvider _collisionEventsProvider;

        private LevelConfig _config;

        public event Action<LevelBlock> OnEnter;

        public bool IsEmpty => _obstacles.Count == 0;

        public void Initialize(LevelConfig config)
        {
            _config = config;
            _scaledContent.localScale = Vector3.one + Vector3.forward * config.BlockSize.y * config.ObstaclePlacingStepZ;
        }
        
        public void AddObstacle(Obstacle obstacle, int x, int z)
        {
            var obstacleTransform = obstacle.transform;
            var localPosition = GetLocalPosition(x, z);

            obstacleTransform.SetParent(transform);
            obstacleTransform.localPosition = localPosition;
            
            _obstacles.Add(obstacle);
        }

        public void AddBuff(CollectableBuff buff, int x, int z)
        {
            var buffTransform = buff.transform;
            var localPosition = GetLocalPosition(x, z);

            buffTransform.SetParent(transform);
            buffTransform.localPosition = localPosition;
        }

        private Vector3 GetLocalPosition(int x, int z)
        {
            var sizeX = _config.BlockSize.x;
            var middleIndexX = sizeX / 2;
            var offsetX = _config.ObstacleOffsetX;
            var resultOffsetX = (x - middleIndexX) * offsetX;
            var resultOffsetZ = z * _config.ObstaclePlacingStepZ;
            var localPosition = new Vector3(resultOffsetX, 0f, resultOffsetZ);
            return localPosition;
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