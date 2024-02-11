using System;
using System.Threading.Tasks;
using CodeBase.Gameplay.StaticData;

namespace CodeBase.Gameplay.Controllers
{
    public interface ILevelController : IController
    {
        event Action OnObstacleHit;
        event Action<BuffConfig> OnBuffCollided;

        Task CreateLevel();
        void DestroyLevel();
    }
}