using Project.CodeBase.Core.Services.GameSession;
using Project.CodeBase.Core.Services.UI;
using Project.CodeBase.Core.StateMachine;
using Project.CodeBase.Gameplay.UI;

namespace Project.CodeBase.Gameplay.StateMachine.States
{
    public sealed class MenuState : StateBase
    {
        private readonly IUIService _uiService;
        private readonly IGameSessionService _gameSessionService;

        private MenuView _menuView;

        public MenuState(IUIService uiService, IGameSessionService gameSessionService)
        {
            _uiService = uiService;
            _gameSessionService = gameSessionService;
        }

        public override async void Enter()
        {
            var showInstantly = _gameSessionService.RunsCount == 0;
            var @params = new MenuView.Params(showInstantly);

            _menuView = await _uiService.Show<MenuView, MenuView.Params>(@params);

            _menuView.OnPlayPressed += HandlePlayPressed;
        }

        public override void Exit()
        {
            _menuView.OnPlayPressed -= HandlePlayPressed;

            _menuView.HideAnimated();

            _menuView = null;
        }

        private void HandlePlayPressed()
        {
            StateMachine.Enter<LoadGameSceneState>();
        }
    }
}