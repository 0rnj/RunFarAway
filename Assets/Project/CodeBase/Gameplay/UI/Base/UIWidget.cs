using System.Threading.Tasks;
using CodeBase.Gameplay.Services.UI;
using UnityEngine;
using VContainer;

namespace CodeBase.Gameplay.UI.Base
{
    [DisallowMultipleComponent]
    public abstract class UIWidget : MonoBehaviour
    {
        protected IUIService UIService;
        protected bool IsActivated { get; private set; }

        private RectTransform _rectTransform;

        public RectTransform RectTransform => _rectTransform ??= transform as RectTransform;

        [Inject]
        private void Construct(IUIService uiService)
        {
            UIService = uiService;
        }

        public void Activate()
        {
            if (IsActivated)
            {
                return;
            }
            
            IsActivated = true;
            
            gameObject.SetActive(true);

            OnActivate();
        }

        public void Deactivate()
        {
            if (IsActivated == false)
            {
                return;
            }
            
            OnDeactivate();

            gameObject.SetActive(false);
            
            IsActivated = false;
        }

        protected virtual void OnActivate() { }

        protected virtual void OnDeactivate() { }

        protected Task<T> Show<T>() where T : UIView
        {
            return UIService.Show<T>();
        }

        protected Task<T> Show<T, TParams>(TParams @params) where T : UIView<TParams>
        {
            return UIService.Show<T, TParams>(@params);
        }
    }

    public abstract class UIWidget<T> : UIWidget
    {
        protected T WidgetParams { get; private set; }

        public void SetParams(T widgetParams)
        {
            WidgetParams = widgetParams;

            if (IsActivated)
            {
                OnParamsUpdated();
            }
        }
        
        protected virtual void OnParamsUpdated() { }
    }
}