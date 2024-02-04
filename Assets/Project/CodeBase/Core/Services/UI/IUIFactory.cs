using Project.CodeBase.Core.Factories;
using Project.CodeBase.Gameplay.UI.Base;
using UnityEngine;

namespace Project.CodeBase.Core.Services.UI
{
    public interface IUIFactory : IFactory
    {
        T Instantiate<T>(T prefab, RectTransform parent) where T : UIWidget;
    }
}