using System;
using System.Threading.Tasks;

namespace CodeBase.Gameplay.Controllers
{
    public interface IGameController : IController
    {
        event Action OnPlayerDied;
        
        Task StartGame();
        void EndGame();
    }
}