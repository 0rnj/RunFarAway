using Project.CodeBase.Core.Services.UI;
using Project.CodeBase.Core.StateMachine;
using Project.CodeBase.Gameplay.UI;

namespace Project.CodeBase.Gameplay.StateMachine.States
{
    public sealed class PlayState : StateBase
    {
        private readonly IUIService _uiService;
        private readonly IGameController _gameController;

        private GameView _gameView;

        public PlayState(IUIService uiService, IGameController gameController)
        {
            _uiService = uiService;
            _gameController = gameController;
        }

        public override async void Enter()
        {
            _gameController.OnPlayerDied += HandlePlayerDied;

            _gameView = await _uiService.Show<GameView>();
        }

        public override void Exit()
        {
            _gameController.OnPlayerDied -= HandlePlayerDied;

            _gameView.HideAnimated();

            _gameView = null;
        }

        private void HandlePlayerDied()
        {
            StateMachine.Enter<EndGameState>();
        }
    }
}