using System;
using System.Threading.Tasks;
using Project.CodeBase.Core.Factories;
using Project.CodeBase.Core.Services.Configs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.CodeBase.Gameplay.Level
{
    public sealed class LevelController : ILevelController, IDisposable
    {
        private readonly IConfigsService _configsService;
        private readonly IObjectFactory _objectFactory;

        public event Action OnObstacleHit;

        public LevelController(IConfigsService configsService, IObjectFactory objectFactory)
        {
            _configsService = configsService;
            _objectFactory = objectFactory;
        }

        void IDisposable.Dispose()
        {
            OnObstacleHit = null;
        }

        public async Task CreateEmptyBlock(int blockIndex)
        {
            await AddBlock(blockIndex);
        }

        public async Task CreateBlock(int blockIndex)
        {
            // var spacing = _config.SpaceForManeuverZ;
            // var obstaclesX = ArrayPool<Obstacle>.Shared.Rent(blockSize.x);
            // var obstaclesY = ArrayPool<Obstacle[]>.Shared.Rent(blockSize.y);

            var blockSize = _configsService.LevelConfig.BlockSize;
            var corridorIndexX = Random.Range(0, blockSize.x);
            var block = await AddBlock(blockIndex);

            for (var x = 0; x < blockSize.x; x++)
            {
                for (var y = 0; y < blockSize.y; y++)
                {
                    if (x != corridorIndexX)
                    {
                        await AddObstacle(block, x, y);
                    }
                }
            }
        }

        private async Task<LevelBlock> AddBlock(int blockIndex)
        {
            var blockSize = _configsService.LevelConfig.BlockSize;
            var blockRef = _configsService.LevelConfig.BlockRef;
            var block = await _objectFactory.CreateComponentGameObject(this, blockRef);
            
            block.Initialize(_configsService.LevelConfig);
            block.transform.localPosition = new Vector2(0f, blockSize.y * blockIndex);
            
            return block;
        }

        private async Task AddObstacle(LevelBlock block, int x, int y)
        {
            var obstacleRef = _configsService.LevelConfig.ObstacleRef;
            var obstacle = await _objectFactory.CreateComponentGameObject(this, obstacleRef);
            
            block.AddObstacle(obstacle, x, y);

            obstacle.OnCollide += HandleObstacleCollided;
        }

        private void HandleObstacleCollided()
        {
            OnObstacleHit?.Invoke();
        }
    }
}