using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeBase.Core.Services;
using CodeBase.Core.StateMachine;
using UnityEngine;
using UnityEngine.Pool;

namespace CodeBase.Gameplay.StateMachine.States
{
    public class BootstrapState : StateBase
    {
        private readonly IEnumerable<IInitializable> _services;

        public BootstrapState(IEnumerable<IInitializable> services)
        {
            _services = services;
        }

        public override async void Enter()
        {
            var allInitialized = await InitializeServices();
            if (allInitialized)
            {
                StateMachine.Enter<LoadMenuSceneState>();
            }
        }

        public override void Exit() { }

        private async Task<bool> InitializeServices()
        {
            var orderedServiceGroups = _services;
            // var orderedServiceGroups = _services
            //     .GroupBy(service => service.InitOrder)
            //     .OrderBy(services => services.Key);

            var tasks = ListPool<Task<bool>>.Get();

            foreach (var serviceGroup in orderedServiceGroups)
            {
                // foreach (var service in serviceGroup)
                {
                    // tasks.Add(service.Initialize());
                    tasks.Add(serviceGroup.Initialize());
                }

                var initResults = await Task.WhenAll(tasks);
                if (initResults.All(isInitialized => isInitialized))
                {
                    continue;
                }

                Debug.LogError("Not all tasks are initialized");
                return false;
            }

            ListPool<Task<bool>>.Release(tasks);

            return true;
        }
    }
}