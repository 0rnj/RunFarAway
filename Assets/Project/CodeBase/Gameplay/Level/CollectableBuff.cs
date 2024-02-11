using System;
using CodeBase.Gameplay.Common;
using CodeBase.Gameplay.StaticData;
using UnityEngine;

namespace CodeBase.Gameplay.Level
{
    public sealed class CollectableBuff : MonoBehaviour
    {
        [SerializeField] private CollisionEventsProvider _collisionEventsProvider;

        public BuffConfig Config { get; private set; }

        public event Action<CollectableBuff> OnBuffCollided; 

        public void Initialize(BuffConfig buffConfig)
        {
            Config = buffConfig;
        }

        private void OnEnable()
        {
            _collisionEventsProvider.OnCollide += HandleCollide;
        }

        private void OnDisable()
        {
            _collisionEventsProvider.OnCollide -= HandleCollide;
        }

        private void HandleCollide()
        {
            OnBuffCollided?.Invoke(this);
        }
    }
}