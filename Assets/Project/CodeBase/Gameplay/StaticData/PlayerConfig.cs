using DG.Tweening;
using Project.CodeBase.Gameplay.Player;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Project.CodeBase.Gameplay.StaticData
{
    [CreateAssetMenu(menuName = "StaticData/PlayerConfig", fileName = "PlayerConfig", order = 0)]
    public sealed class PlayerConfig : ScriptableObject
    {
        [field: SerializeField] public int Hp { get; private set; }
        [field: SerializeField] public float StartingMoveSpeed { get; private set; }
        [field: SerializeField] public float MoveSpeedGain { get; private set; }
        [field: SerializeField] public float MoveSpeedGainInterval { get; private set; }
        [field: SerializeField] public float StrafeDuration { get; private set; }
        [field: SerializeField] public Ease StrafeEase { get; private set; }
        [field: SerializeField] public AssetReferenceT<PlayerVisuals> PlayerVisualsRef { get; private set; }
    }
}