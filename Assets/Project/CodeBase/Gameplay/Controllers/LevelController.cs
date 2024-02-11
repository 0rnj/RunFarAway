using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeBase.Core.Factories;
using CodeBase.Gameplay.Enums;
using CodeBase.Gameplay.Scene.Level;
using CodeBase.Gameplay.Services.Configs;
using CodeBase.Gameplay.StaticData;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace CodeBase.Gameplay.Controllers
{
    public sealed class LevelController : ILevelController, IDisposable
    {
        private readonly IConfigsService _configsService;
        private readonly IObjectFactory _objectFactory;
        private readonly List<LevelBlock> _levelBlocks = new();

        private int _currentBlockIndex;
        private int _previousCorridorIndexX = -1;

        public event Action OnObstacleHit;
        public event Action<BuffConfig> OnBuffCollided;

        public LevelController(IConfigsService configsService, IObjectFactory objectFactory)
        {
            _configsService = configsService;
            _objectFactory = objectFactory;
        }

        void IDisposable.Dispose()
        {
            OnObstacleHit = null;
        }

        public async Task CreateLevel()
        {
            _currentBlockIndex = -2;

            var emptyCount = _configsService.LevelConfig.StartingEmptyBlocksCount;

            for (var i = 0; i < emptyCount; i++)
            {
                await CreateEmptyBlock();
            }

            var blocksCount = _configsService.LevelConfig.StartingBlocksCount;

            for (var i = 0; i < blocksCount; i++)
            {
                await CreateBlock();
            }
        }

        public void DestroyLevel()
        {
            for (var i = 0; i < _levelBlocks.Count; i++)
            {
                var levelBlock = _levelBlocks[i];

                levelBlock.OnEnter -= HandleBlockEntered;

                Object.Destroy(levelBlock.gameObject);
            }

            _levelBlocks.Clear();
        }

        public async Task CreateEmptyBlock()
        {
            await AddBlock();
        }

        public async Task CreateBlock()
        {
            var blockSize = _configsService.LevelConfig.BlockSize;

            var block = await AddBlock();

            for (var z = 0; z < blockSize.y; z++)
            {
                var corridorIndexX = GetCorridorIndexX(blockSize);

                for (var x = 0; x < blockSize.x; x++)
                {
                    if (x == corridorIndexX)
                    {
                        await TryAddBuff(block, x, z);
                    }
                    else if (ShouldAddObstacle())
                    {
                        await AddObstacle(block, x, z);
                    }
                }
            }
        }

        private bool ShouldAddObstacle()
        {
            var rnd = Random.Range(0f, 1f);
            return rnd > _configsService.LevelConfig.NoObstacleChance;
        }

        private int GetCorridorIndexX(Vector2Int blockSize)
        {
            var corridorIndices = ListPool<int>.Get();

            for (var i = 0; i < blockSize.x; i++)
            {
                if (i != _previousCorridorIndexX)
                {
                    corridorIndices.Add(i);
                }
            }

            var rnd = Random.Range(0, corridorIndices.Count);
            var corridorIndexX = corridorIndices[rnd];

            corridorIndices.Clear();
            ListPool<int>.Release(corridorIndices);

            _previousCorridorIndexX = corridorIndexX;

            return corridorIndexX;
        }

        private async Task<LevelBlock> AddBlock()
        {
            var levelConfig = _configsService.LevelConfig;
            var blockSize = levelConfig.BlockSize;
            var blockRef = levelConfig.BlockRef;
            var block = await _objectFactory.Create<LevelBlock>(this, blockRef);

            var z = blockSize.y * _currentBlockIndex * levelConfig.ObstaclePlacingStepZ;
            block.Initialize(levelConfig);
            block.transform.localPosition = new Vector3(0f, 0f, z);

            block.OnEnter += HandleBlockEntered;

            _levelBlocks.Add(block);
            _currentBlockIndex++;

            return block;
        }

        private async Task AddObstacle(LevelBlock block, int x, int z)
        {
            var obstacleRef = _configsService.LevelConfig.ObstacleRef;
            var obstacle = await _objectFactory.Create<Obstacle>(this, obstacleRef);

            block.AddObstacle(obstacle, x, z);

            obstacle.OnCollide += HandleObstacleCollided;
        }

        private async Task TryAddBuff(LevelBlock block, int x, int z)
        {
            var buffConfigs = _configsService.BuffsConfig.Buffs;
            var totalWeight = buffConfigs.Sum(config => config.Weight);

            var weight = 1 + Random.Range(0, totalWeight);

            for (var i = 0; i < buffConfigs.Count; i++)
            {
                var buffConfig = buffConfigs[i];
                var buffWeight = buffConfig.Weight;
                if (weight > buffWeight)
                {
                    weight -= buffWeight;
                    continue;
                }

                if (buffConfig.BuffType == BuffType.None)
                {
                    return;
                }

                var collectableBuff = await _objectFactory.Create<CollectableBuff>(this, buffConfig.VisualsRef);

                collectableBuff.Initialize(buffConfig);

                collectableBuff.OnBuffCollided += HandleBuffCollected;

                block.AddBuff(collectableBuff, x, z);
                break;
            }
        }

        private async void HandleBlockEntered(LevelBlock levelBlock)
        {
            var removedBlock = _levelBlocks[0];
            var blockIsEmpty = removedBlock.IsEmpty;
            _levelBlocks.RemoveAt(0);

            removedBlock.OnEnter -= HandleBlockEntered;

            Object.Destroy(removedBlock.gameObject);

            if (blockIsEmpty)
            {
                return;
            }

            await CreateBlock();
        }

        private void HandleObstacleCollided()
        {
            OnObstacleHit?.Invoke();
        }

        private void HandleBuffCollected(CollectableBuff collectableBuff)
        {
            OnBuffCollided?.Invoke(collectableBuff.Config);

            Object.Destroy(collectableBuff.gameObject);
        }
    }
}