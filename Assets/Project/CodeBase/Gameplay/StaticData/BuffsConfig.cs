using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Gameplay.StaticData
{
    [CreateAssetMenu(menuName = "StaticData/BuffsConfig", fileName = "BuffsConfig", order = 0)]
    public sealed class BuffsConfig : ScriptableObject
    {
        [field: SerializeField] public float SlowPower { get; private set; }
        [field: SerializeField] public float SpeedUpPower { get; private set; }
        
        [SerializeField] private List<BuffConfig> _buffConfigs;
        
        public IReadOnlyList<BuffConfig> Buffs => _buffConfigs;
    }
}