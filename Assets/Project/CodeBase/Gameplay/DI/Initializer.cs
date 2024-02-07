using CodeBase.Gameplay.StateMachine;
using CodeBase.Gameplay.StateMachine.States;
using VContainer.Unity;

namespace CodeBase.Gameplay.DI
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