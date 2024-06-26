﻿using System;
using UnityEngine;

namespace CodeBase.Gameplay.Scene.Level
{
    [RequireComponent(typeof(Collider))]
    public sealed class CollisionEventsProvider : MonoBehaviour
    {
        public event Action OnCollide;
        
        private void OnCollisionEnter(Collision other)
        {
            OnCollide?.Invoke();
        }

        private void OnTriggerEnter(Collider other)
        {
            OnCollide?.Invoke();
        }
    }
}