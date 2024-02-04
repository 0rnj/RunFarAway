using CodeBase.Gameplay.Services.Configs;

namespace CodeBase.Gameplay.StateMachine.States
{
    public sealed class LoadMenuSceneState : LoadSceneState
    {
        protected override string SceneName => ConfigsService.GameConfig.MenuSceneName;

        public LoadMenuSceneState(IConfigsService configsService) : base(configsService) { }

        protected override void Proceed()
        {
            
        }

        public override void Exit()
        {
            
        }
    }
}