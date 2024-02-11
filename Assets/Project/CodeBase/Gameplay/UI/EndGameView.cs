using System;
using CodeBase.Gameplay.UI.Base;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.Gameplay.UI
{
    public sealed class EndGameView : UIView
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        
        private Sequence _sequence;

        public event Action OnAnimationComplete;
        
        protected override void OnActivateView()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence()
                .Append(_canvasGroup.DOFade(1f, 0.5f).From(0f))
                .AppendInterval(1f)
                .Append(_canvasGroup.DOFade(0f, 0.5f))
                .OnComplete(() => OnAnimationComplete?.Invoke())
                .Play();
        }
    }
}