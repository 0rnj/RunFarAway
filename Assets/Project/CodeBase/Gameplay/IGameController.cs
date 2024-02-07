using System;
using System.Threading.Tasks;

namespace CodeBase.Gameplay
{
    public interface IGameController : IController
    {
        event Action OnPlayerDied;
        
        Task StartGame();
    }
}