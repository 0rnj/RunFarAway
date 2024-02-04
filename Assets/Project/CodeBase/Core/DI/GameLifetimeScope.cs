using Project.CodeBase.Core.StateMachine.Interfaces;
using Project.CodeBase.Gameplay.StateMachine;
using Project.CodeBase.Gameplay.StateMachine.States;
using VContainer;
using VContainer.Unity;

namespace Project.CodeBase.Core.DI
{
    public class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            var gameExportsCollection = new GameExportsCollection();

            foreach (var exportedType in gameExportsCollection.GetExportedTypes())
            {
                builder.Register(exportedType, Lifetime.Singleton).AsImplementedInterfaces();
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

                if (state is IInstalledWith<GameStateMachine> installedWith)
                {
                    installedWith.Install(gameStateMachine);
                }

                gameStateMachine.AddState(state);
            }
        }
    }
}