using System.Threading.Tasks;
using CodeBase.Gameplay.UI.Base;
using UnityEngine;

namespace CodeBase.Gameplay.Services.UI
{
    public interface IUIService
    {
        T GetView<T>() where T : UIView;

        Task<T> Show<T>() where T : UIView;

        Task<T> Show<T, TParams>(TParams @params) where T : UIView<TParams>;

        void Hide<T>() where T : UIView;

        void Hide(UIView view);

        Task<T> CreateWidget<T>(RectTransform parent) where T : UIWidget;

        Task<T> CreateWidget<T, TParams>(RectTransform parent, TParams @params) where T : UIWidget<TParams>;

        void DestroyWidget<T>(T widget) where T : UIWidget;
    }
}