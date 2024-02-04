using System;
using Project.CodeBase.Gameplay.Common;
using UnityEngine;

namespace Project.CodeBase.Gameplay.Level
{
    public class Obstacle : MonoBehaviour
    {
        public event Action OnCollide;

        [SerializeField] private CollisionEventsProvider _collisionEventsProvider;

        public void Setup(float sizeZ)
        {
            transform.localScale = Vector3.one + Vector3.forward * sizeZ;
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
            OnCollide?.Invoke();
        }
    }
}