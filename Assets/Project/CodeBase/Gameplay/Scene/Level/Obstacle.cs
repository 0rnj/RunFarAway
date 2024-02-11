using System;
using UnityEngine;

namespace CodeBase.Gameplay.Scene.Level
{
    public class Obstacle : MonoBehaviour
    {
        public event Action OnCollide;

        [SerializeField] private CollisionEventsProvider _collisionEventsProvider;

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
            OnCollide?.Invoke();
        }
    }
}