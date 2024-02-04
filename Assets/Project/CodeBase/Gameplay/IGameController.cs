using System;
using System.Threading.Tasks;

namespace Project.CodeBase.Gameplay
{
    public interface IGameController
    {
        Task StartGame();
        event Action OnPlayerDied;
    }
}