using Project.CodeBase.Core.Services.UI;
using Project.CodeBase.Core.StateMachine;
using Project.CodeBase.Gameplay.UI;

namespace Project.CodeBase.Gameplay.StateMachine.States
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