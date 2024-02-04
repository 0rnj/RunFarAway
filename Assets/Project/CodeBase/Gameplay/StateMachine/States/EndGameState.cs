using CodeBase.Core.StateMachine;
using CodeBase.Gameplay.Services.UI;
using CodeBase.Gameplay.UI;

namespace CodeBase.Gameplay.StateMachine.States
{
    public sealed class EndGameState : StateBase
    {
        private readonly IUIService _uiService;
        
        public EndGameState(IUIService uiService)
        {
            _uiService = uiService;
        }

        public override void Enter()
        {
            var menuView = _uiService.GetView<MenuView>();
        }

        public override void Exit()
        {
            
        }
    }
}