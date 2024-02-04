using System;
using System.Threading.Tasks;

namespace CodeBase.Gameplay.Level
{
    public interface ILevelController
    {
        Task CreateBlock(int blockIndex);
        Task CreateEmptyBlock(int blockIndex);
        event Action OnObstacleHit;
    }
}