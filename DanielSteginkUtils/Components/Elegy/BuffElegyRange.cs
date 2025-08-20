using UnityEngine;

namespace DanielSteginkUtils.Components.Dung
{
    /// <summary>
    /// Custom component for modifying the size of Grubberfly's Elegy beam attacks
    /// </summary>
    public class BuffElegyRange : ModBuffs<Vector3>
    {
        /// <summary>
        /// Applies modifiers to base value
        /// </summary>
        /// <returns></returns>
        public override Vector3 GetModdedValue()
        {
            return new Vector3(BaseValue.x * GetModifier(), BaseValue.y * GetModifier());
        }
    }
}