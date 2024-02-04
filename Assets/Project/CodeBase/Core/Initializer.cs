using Project.CodeBase.Gameplay.StateMachine;
using Project.CodeBase.Gameplay.StateMachine.States;
using VContainer.Unity;

namespace Project.CodeBase.Core
{
    public sealed class Initializer : IInitializable
    {
        private readonly GameStateMachine _gameStateMachine;

        public Initializer(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public void Initialize()
        {
            _gameStateMachine.Enter<BootstrapState>();
        }
    }
}