using System;
using CodeBase.Gameplay.UI;
using CodeBase.Gameplay.UI.Base;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodeBase.Gameplay.Services.Input.Providers
{
    public sealed class MobileInputProvider : UIView, IInputProvider, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        public event Action OnJump;
        public event Action<StrafeDirection> OnStrafe;

        [SerializeField] private Image _image;
        [SerializeField] private float _swipeThreshold;

        private bool _isDragging;
        private Vector2 _pointerStartPosition;

        public void SetActive(bool active)
        {
            _image.enabled = active;
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            _isDragging = true;
            _pointerStartPosition = eventData.position;
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            //
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            _isDragging = false;

            var pointerEndPosition = eventData.position;
            var delta = pointerEndPosition.x - _pointerStartPosition.x;

            if (Mathf.Abs(delta) < _swipeThreshold)
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