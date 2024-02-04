using System;
using System.Threading.Tasks;

namespace CodeBase.Gameplay
{
    public interface IGameController
    {
        Task StartGame();
        event Action OnPlayerDied;
    }
}