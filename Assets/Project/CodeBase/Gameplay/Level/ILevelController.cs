using System;
using System.Threading.Tasks;

namespace CodeBase.Gameplay.Level
{
    public interface ILevelController : IController
    {
        Task CreateBlock();
        Task CreateEmptyBlock();
        event Action OnObstacleHit;
        Task CreateLevel();
    }
}