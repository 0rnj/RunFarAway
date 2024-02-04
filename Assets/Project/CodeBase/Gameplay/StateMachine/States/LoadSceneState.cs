using Project.CodeBase.Core.Services.Configs;
using Project.CodeBase.Core.StateMachine;
using UnityEngine.SceneManagement;

namespace Project.CodeBase.Gameplay.StateMachine.States
{
    public abstract class LoadSceneState : StateBase
    {
        protected readonly IConfigsService ConfigsService;
        
        protected abstract string SceneName { get; }

        protected LoadSceneState(IConfigsService configsService)
        {
            ConfigsService = configsService;
        }

        public override void Enter()
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
            SceneManager.sceneLoaded += HandleSceneLoaded;

            SceneManager.LoadScene(SceneName);
        }

        public override void Exit() { }

        protected abstract void Proceed();
        
        private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;

            Proceed();
        }
    }
}