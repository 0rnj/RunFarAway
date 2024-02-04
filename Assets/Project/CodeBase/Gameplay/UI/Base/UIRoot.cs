using UnityEngine;

namespace Project.CodeBase.Gameplay.UI.Base
{
    public sealed class UIRoot : MonoBehaviour
    {
        [field: SerializeField] public RectTransform Pivot { get; private set; }
    }
}