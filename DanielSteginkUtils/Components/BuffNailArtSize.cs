using UnityEngine;

namespace DanielSteginkUtils.Components
{
    /// <summary>
    /// Custom component for modifying the size of clouds created by Nail Art attacks
    /// </summary>
    public class BuffNailArtSize : ModBuffs<Vector3>
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