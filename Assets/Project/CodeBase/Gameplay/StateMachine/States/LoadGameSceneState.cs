using CodeBase.Gameplay.Services.Configs;

namespace CodeBase.Gameplay.StateMachine.States
{
    public sealed class LoadGameSceneState : LoadSceneState
    {
        protected override string SceneName => ConfigsService.GameConfig.GameSceneName;
        
        public LoadGameSceneState(IConfigsService configsService) : base(configsService) { }

        protected override void Proceed()
        {
            StateMachine.Enter<PlayState>();
        }
    }
}