using CodeBase.Core.StateMachine;
using CodeBase.Gameplay.Services.UI;
using CodeBase.Gameplay.UI;

namespace CodeBase.Gameplay.StateMachine.States
{
    public sealed class MenuState : StateBase
    {
        private readonly IUIService _uiService;

        private MenuView _menuView;

        public MenuState(IUIService uiService)
        {
            _uiService = uiService;
        }

        public override async void Enter()
        {
            _menuView = await _uiService.Show<MenuView>();

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