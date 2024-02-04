using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

namespace Project.CodeBase.Gameplay.UI.Base
{
    public abstract class UIView : UIWidget
    {
        private List<UIWidget> _widgets;

        protected sealed override void OnActivate()
        {
            OnActivateView();
        }

        protected sealed override void OnDeactivate()
        {
            if (_widgets is { Count: > 0 })
            {
                for (var i = 0; i < _widgets.Count; i++)
                {
                    var widget = _widgets[i];
                    DestroyWidget(widget);
                }

                _widgets.Clear();
                ListPool<UIWidget>.Release(_widgets);
                _widgets = null;
            }

            OnDeactivateView();
        }

        protected async Task<T> CreateWidget<T>(RectTransform parent)
            where T : UIWidget
        {
            var widget = await UIService.CreateWidget<T>(parent);

            _widgets ??= ListPool<UIWidget>.Get();
            _widgets.Add(widget);

            return widget;
        }

        protected async Task<T> CreateWidget<T, TParams>(RectTransform parent, TParams @params)
            where T : UIWidget<TParams>
        {
            var widget = await UIService.CreateWidget<T, TParams>(parent, @params);

            _widgets ??= ListPool<UIWidget>.Get();
            _widgets.Add(widget);

            return widget;
        }

        protected void DestroyWidget(UIWidget widget)
        {
            _widgets.Remove(widget);
            UIService.DestroyWidget(widget);
        }

        protected void Close()
        {
            UIService.Hide(this);
        }

        protected virtual void OnActivateView() { }
        protected virtual void OnDeactivateView() { }
    }

    public abstract class UIView<T> : UIView
    {
        protected T ViewParams;

        public void SetParams(T viewParams)
        {
            ViewParams = viewParams;

            if (IsActivated)
            {
                OnParamsSet();
            }
        }

        protected virtual void OnParamsSet() { }
    }
}