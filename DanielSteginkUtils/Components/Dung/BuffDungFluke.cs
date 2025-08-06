using DanielSteginkUtils.Components.Template;

namespace DanielSteginkUtils.Components.Dung
{
    /// <summary>
    /// Custom component for modifying the damage rate of clouds created by Dung Flukes
    /// </summary>
    public class BuffDungFluke : ModBuffs<float>
    {
        /// <summary>
        /// Applies modifiers to base value
        /// </summary>
        /// <returns></returns>
        public override float GetModdedValue()
        {
            return BaseValue * GetModifier();
        }
    }
}