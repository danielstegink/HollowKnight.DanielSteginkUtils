using DanielSteginkUtils.Components.Template;

namespace DanielSteginkUtils.Components.Dung
{
    /// <summary>
    /// Custom component for modifying the damage rate of clouds created by Defender's Crest
    /// </summary>
    public class BuffDungDamage : ModBuffs<float>
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