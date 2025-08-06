using UnityEngine;

namespace DanielSteginkUtils.Components.Dung
{
    /// <summary>
    /// Custom component for modifying the size of clouds created by Defender's Crest
    /// </summary>
    public class BuffDungSize : ModBuffs<Vector3>
    {
        /// <summary>
        /// Applies modifiers to base value
        /// </summary>
        /// <returns></returns>
        public override Vector3 GetModdedValue()
        {
            return BaseValue * GetModifier();
        }
    }
}