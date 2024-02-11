using CodeBase.Core.StateMachine;
using CodeBase.Gameplay.Controllers;
using CodeBase.Gameplay.Services.UI;
using CodeBase.Gameplay.UI;

namespace CodeBase.Gameplay.StateMachine.States
{
    public sealed class EndGameState : StateBase
    {
        private readonly IUIService _uiService;
        private readonly IGameController _gameController;

        private EndGameView _endGameView;

        public EndGameState(IUIService uiService, IGameController gameController)
        {
            _uiService = uiService;
            _gameController = gameController;
        }

        public override async void Enter()
        {
            _endGameView = await _uiService.Show<EndGameView>();

            _endGameView.OnAnimationComplete += HandleAnimationComplete;
        }

        public override void Exit()
        {
            _gameController.EndGame();

            if (_endGameView == null)
            {
                return;
            }

            var endGameView = _endGameView;

            _endGameView = null;

            endGameView.OnAnimationComplete -= HandleAnimationComplete;

            _uiService.Hide(endGameView);
        }

        private void HandleAnimationComplete()
        {
            StateMachine.Enter<LoadMenuSceneState>();
        }
    }
}