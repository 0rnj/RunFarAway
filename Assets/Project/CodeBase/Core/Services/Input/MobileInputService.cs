using System;
using Project.CodeBase.Gameplay.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.CodeBase.Core.Services.Input
{
    public sealed class MobileInputService : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IPointerClickHandler, IInputService
    {
        public event Action OnJump;
        public event Action<StrafeDirection> OnStrafe;

        [SerializeField] private float _swipeThreshold;

        private bool _isDragging;
        private Vector2 _pointerStartPosition;

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            _isDragging = true;
            _pointerStartPosition = eventData.position;
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            _isDragging = false;

            var pointerEndPosition = eventData.position;
            var delta = pointerEndPosition.x - _pointerStartPosition.x;

            if (delta < _swipeThreshold)
            {
                return;
            }

            var strafeDirection = delta < 0f ? StrafeDirection.Left : StrafeDirection.Right;

            OnStrafe?.Invoke(strafeDirection);
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (_isDragging)
            {
                return;
            }

            OnJump?.Invoke();
        }
    }
}