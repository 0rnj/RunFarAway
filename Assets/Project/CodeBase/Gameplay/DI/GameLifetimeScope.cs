using CodeBase.Core.DI;
using CodeBase.Core.StateMachine.Interfaces;
using CodeBase.Gameplay.StateMachine;
using CodeBase.Gameplay.StateMachine.States;
using VContainer;
using VContainer.Unity;

namespace CodeBase.Gameplay.DI
{
    public class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            DontDestroyOnLoad(this);

            var coreExportsCollection = new CoreExportsCollection();
            var gameExportsCollection = new GameExportsCollection();
            var stateExportsCollection = new StateExportsCollection();

            foreach (var exportedType in coreExportsCollection.GetExportedTypes())
            {
                builder.Register(exportedType, Lifetime.Singleton).AsImplementedInterfaces();
            }

            foreach (var exportedType in gameExportsCollection.GetExportedTypes())
            {
                builder.Register(exportedType, Lifetime.Singleton).AsImplementedInterfaces();
            }

            foreach (var exportedType in stateExportsCollection.GetExportedTypes())
            {
                builder.Register(exportedType, Lifetime.Singleton);
            }

            builder.Register(CreateGameStateMachine, Lifetime.Singleton);
        }

        private GameStateMachine CreateGameStateMachine(IObjectResolver container)
        {
            var gameStateMachine = new GameStateMachine();

            AddState<BootstrapState>();
            AddState<LoadMenuSceneState>();
            AddState<LoadGameSceneState>();
            AddState<StartGameState>();
            AddState<MenuState>();
            AddState<PlayState>();
            AddState<EndGameState>();

            return gameStateMachine;

            void AddState<T>() where T : class, IExitableState
            {
                var state = container.Resolve<T>();

                if (state is IInstalledWith<GameStateMachine> installedState)
                {
                    installedState.Install(gameStateMachine);
                }

                gameStateMachine.AddState(state);
            }
        }
    }
}