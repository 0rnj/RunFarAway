using System;
using CodeBase.Gameplay.Enums;
using UnityEngine.AddressableAssets;

namespace CodeBase.Gameplay.StaticData
{
    [Serializable]
    public sealed class BuffConfig
    {
        public BuffType BuffType;
        public float Duration;
        public int Weight;
        public AssetReference VisualsRef;
    }
}