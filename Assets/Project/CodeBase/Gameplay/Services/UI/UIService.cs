using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Core.Services;
using CodeBase.Core.Services.ResourcesLoading;
using CodeBase.Gameplay.UI.Base;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.Gameplay.Services.UI
{
    public sealed class UIService : IUIService, IInitializable
    {
        private readonly IResourcesService _resourcesService;
        private readonly IUIFactory _uiFactory;
        private readonly Dictionary<Type, UIView> _viewPrefabs = new();
        private readonly Dictionary<Type, UIView> _viewInstances = new();
        private readonly Dictionary<Type, UIWidget> _widgetPrefabs = new();

        private UIRoot _uiRoot;

        public UIService(IResourcesService resourcesService, IUIFactory uiFactory)
        {
            _resourcesService = resourcesService;
            _uiFactory = uiFactory;
        }

        public async Task<bool> Initialize()
        {
            var uiRoot = await _resourcesService.LoadAsync<UIRoot>(this, nameof(UIRoot));

            _uiRoot = Object.Instantiate(uiRoot);

            _uiRoot.gameObject.name = "[UI]";

            Object.DontDestroyOnLoad(_uiRoot);
            return true;
        }

        public T GetView<T>()
            where T : UIView
        {
            var type = typeof(T);

            if (_viewInstances.TryGetValue(type, out var view))
            {
                return (T)view;
            }

            Debug.LogError($"No widget of type {type.Name} to get");
            return default;
        }

        public async Task<T> Show<T>()
            where T : UIView
        {
            var type = typeof(T);

            if (_viewInstances.TryGetValue(type, out var existingView))
            {
                return (T)existingView;
            }

            var newView = await InstantiateView<T>();

            newView.Activate();

            return newView;
        }

        public async Task<T> Show<T, TParams>(TParams @params)
            where T : UIView<TParams>
        {
            var type = typeof(T);

            if (_viewInstances.TryGetValue(type, out var existingView))
            {
                return (T)existingView;
            }

            var newView = await InstantiateView<T>();

            newView.SetParams(@params);
            newView.Activate();

            return newView;
        }

        public void Hide<T>()
            where T : UIView
        {
            var type = typeof(T);

            if (_viewInstances.TryGetValue(type, out var view) == false)
            {
                Debug.LogError($"No widget of type {type.Name} to hide");
                return;
            }

            Hide(view);
        }

        public void Hide(UIView view)
        {
            var type = view.GetType();

            view.Deactivate();

            Object.Destroy(view.gameObject);

            _viewInstances.Remove(type);

            if (_viewPrefabs.Remove(type, out var prefab))
            {
                _resourcesService.Release(this, prefab);
            }
        }

        public async Task<T> CreateWidget<T>(RectTransform parent)
            where T : UIWidget
        {
            var widget = await InstantiateWidget<T>(parent);

            widget.Activate();

            return widget;
        }

        public async Task<T> CreateWidget<T, TParams>(RectTransform parent, TParams @params)
            where T : UIWidget<TParams>
        {
            var widget = await InstantiateWidget<T>(parent);

            widget.SetParams(@params);
            widget.Activate();

            return widget;
        }

        public void DestroyWidget<T>(T widget)
            where T : UIWidget
        {
            widget.Deactivate();

            Object.Destroy(widget.gameObject);
        }

        private async Task<T> InstantiateView<T>() where T : UIView
        {
            var type = typeof(T);

            if (_viewPrefabs.TryGetValue(type, out var viewPrefab) == false)
            {
                viewPrefab = await _resourcesService.LoadAsync<T>(this, type.Name);
                _viewPrefabs[type] = viewPrefab;
            }

            var newView = _uiFactory.Instantiate(viewPrefab, _uiRoot.Pivot);

            _viewInstances[type] = newView;

            return (T)newView;
        }

        private async Task<T> InstantiateWidget<T>(RectTransform parent)
            where T : UIWidget
        {
            var type = typeof(T);

            if (_widgetPrefabs.TryGetValue(type, out var widgetPrefab) == false)
            {
                widgetPrefab = await _resourcesService.LoadAsync<T>(this, type.Name);
                _widgetPrefabs[type] = widgetPrefab;
            }

            var widget = _uiFactory.Instantiate(widgetPrefab, parent);

            return (T)widget;
        }
    }
}