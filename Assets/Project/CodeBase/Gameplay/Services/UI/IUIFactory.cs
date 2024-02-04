using CodeBase.Core.Factories;
using CodeBase.Gameplay.UI.Base;
using UnityEngine;

namespace CodeBase.Gameplay.Services.UI
{
    public interface IUIFactory : IFactory
    {
        T Instantiate<T>(T prefab, RectTransform parent) where T : UIWidget;
    }
}