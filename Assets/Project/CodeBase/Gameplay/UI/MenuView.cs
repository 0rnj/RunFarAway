using System;
using CodeBase.Gameplay.UI.Base;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Gameplay.UI
{
    public sealed class MenuView : UIView<MenuView.Params>
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Settings _settings;

        private Sequence _showHideSequence;

        public event Action OnPlayPressed;

        public void HideAnimated()
        {
            SetVisible(false, onComplete: Close);
        }
        
        protected override void OnActivateView()
        {
            _playButton.onClick.AddListener(HandlePlayPressed);

            SetVisible(true, ViewParams.ShowInstantly);
        }

        protected override void OnDeactivateView()
        {
            _playButton.onClick.RemoveListener(HandlePlayPressed);

            _showHideSequence?.Kill();

            OnPlayPressed = null;
        }

        private void SetVisible(bool visible, bool instantly = false, Action onComplete = null)
        {
            _showHideSequence?.Kill();

            var endValue = visible ? 1f : 0f;
            
            if (instantly)
            {
                _canvasGroup.alpha = endValue;
                return;
            }

            var startValue = visible ? 0f : 1f;

            _showHideSequence = DOTween.Sequence()
                .Append(_canvasGroup.DOFade(endValue, _settings.ShowHideDuration)
                    .From(startValue)
                    .SetEase(_settings.ShowHideEase))
                .OnComplete(() => onComplete?.Invoke())
                .Play();
        }

        private void HandlePlayPressed()
        {
            OnPlayPressed?.Invoke();
        }

        [Serializable]
        private sealed class Settings
        {
            [field: SerializeField] public float ShowHideDuration { get; private set; }
            [field: SerializeField] public Ease ShowHideEase { get; private set; }
        }
        
        public class Params
        {
            public readonly bool ShowInstantly;

            public Params(bool showInstantly)
            {
                ShowInstantly = showInstantly;
            }
        }
    }
}