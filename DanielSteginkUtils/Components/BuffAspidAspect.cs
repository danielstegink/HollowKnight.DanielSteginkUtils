using DanielSteginkUtils.Utilities;

namespace DanielSteginkUtils.Components
{
    /// <summary>
    /// Custom component for modifying the damage of companions created by Aspid Aspect
    /// </summary>
    public class BuffAspidAspect : ModBuffs<int>
    {
        /// <summary>
        /// Applies modifiers to base value
        /// </summary>
        /// <returns></returns>
        public override int GetModdedValue()
        {
            return Calculations.GetModdedInt(BaseValue, GetModifier());
        }
    }
}
