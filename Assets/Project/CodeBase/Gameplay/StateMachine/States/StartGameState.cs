using CodeBase.Core.StateMachine;

namespace CodeBase.Gameplay.StateMachine.States
{
    public sealed class StartGameState : StateBase
    {
        private readonly IGameController _gameController;

        public StartGameState(IGameController gameController)
        {
            _gameController = gameController;
        }

        public override async void Enter()
        {
            await _gameController.StartGame();

            StateMachine.Enter<PlayState>();
        }

        public override void Exit() { }
    }
}