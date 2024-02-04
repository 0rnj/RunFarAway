using UnityEngine;

namespace Project.CodeBase.Gameplay.StaticData
{
    [CreateAssetMenu(menuName = "StaticData/GameConfig", fileName = "GameConfig", order = 0)]
    public sealed class GameConfig : ScriptableObject
    {
        [field: SerializeField] public string MenuSceneName { get; private set; }
        [field: SerializeField] public string GameSceneName { get; private set; }
    }
}