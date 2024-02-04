using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Project.CodeBase.Core.Services.ResourcesLoading
{
    internal class AsyncOperationsContainer
    {
        private readonly List<AsyncOperationHandle> _handlers = new();
        private readonly List<WeakReference<object>> _owners = new();
        private readonly string _primaryKey;

        public AsyncOperationsContainer(string primaryKey)
        {
            _primaryKey = primaryKey;
        }

        public void Add(object owner, AsyncOperationHandle operation)
        {
            _owners.Add(new WeakReference<object>(owner));
            _handlers.Add(operation);
        }

        public void Release(object owner, bool once = false)
        {
            for (var i = 0; i < _owners.Count; i++)
            {
                var weakReference = _owners[i];
                if (weakReference.TryGetTarget(out var target) && target != owner)
                {
                    continue;
                }

                _owners.RemoveAt(i);
                i--;

                if (once)
                {
                    break;
                }
            }

            if (_owners.Count == 0)
            {
                ReleaseAll();
            }
        }

        public void ReleaseAll()
        {
            _owners.Clear();

            if (!_handlers.Any())
            {
                return;
            }

            for (var i = 0; i < _handlers.Count; i++)
            {
                var asyncOperationHandle = _handlers[i];
                if (asyncOperationHandle.IsValid())
                {
                    UnityEngine.AddressableAssets.Addressables.Release(asyncOperationHandle);
                }
            }

            _handlers.Clear();
// #if DEBUG
//             _logger.Info($"[{GetType().Name}] [{nameof(ReleaseAll)}] Released async handlers for: {_primaryKey}");
// #endif
        }
    }
}