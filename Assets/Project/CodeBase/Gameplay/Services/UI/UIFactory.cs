using CodeBase.Gameplay.UI.Base;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CodeBase.Gameplay.Services.UI
{
    public sealed class UIFactory : IUIFactory
    {
        private readonly IObjectResolver _objectResolver;

        public UIFactory(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }

        public T Instantiate<T>(T prefab, RectTransform parent) where T : UIWidget
        {
            var widget = _objectResolver.Instantiate(prefab, parent);
            var widgetTransform = widget.transform;

            widgetTransform.localPosition = Vector3.zero;
            widgetTransform.localRotation = Quaternion.identity;
            widgetTransform.localScale = Vector3.one;

            return widget;
        }
    }
}