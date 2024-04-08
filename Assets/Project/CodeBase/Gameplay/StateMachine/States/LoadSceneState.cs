using CodeBase.Core.StateMachine;
using CodeBase.Gameplay.Services.Configs;
using UnityEngine.SceneManagement;

namespace CodeBase.Gameplay.StateMachine.States
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

        private void HandleSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;

            Proceed();
        }
    }
}