using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Gameplay.StaticData
{
    [CreateAssetMenu(menuName = "StaticData/PlayerConfig", fileName = "PlayerConfig", order = 0)]
    public sealed class PlayerConfig : ScriptableObject
    {
        [field: SerializeField] public int Hp { get; private set; }
        [field: SerializeField] public float StartingMoveSpeed { get; private set; }
        [field: SerializeField] public float MoveSpeedGain { get; private set; }
        [field: SerializeField] public float MoveSpeedGainInterval { get; private set; }
        [field: SerializeField] public float MinMoveSpeed { get; private set; }
        [field: SerializeField] public float StrafeDuration { get; private set; }
        [field: SerializeField] public float SpeedMultiplierWhileStrafing { get; private set; }
        [field: SerializeField] public Ease StrafeEase { get; private set; }
        [field: SerializeField] public float FlightHeight { get; private set; }
        [field: SerializeField] public AssetReference PlayerVisualsRef { get; private set; }
    }
}