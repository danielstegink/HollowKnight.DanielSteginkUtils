using DanielSteginkUtils.Components;
using Modding.Utils;

namespace DanielSteginkUtils.Helpers.Abilities
{
    /// <summary>
    /// Helper for the ExtraJumps component, which handles extra jumps for the player
    /// </summary>
    public class JumpHelper
    {
        #region Variables
        /// <summary>
        /// Name of the calling mod
        /// </summary>
        public string modName { get; set; } = "";

        /// <summary>
        /// Name of the calling feature
        /// </summary>
        public string featureName { get; set; } = "";

        /// <summary>
        /// How many extra jumps this mod adds the the ExtraJumps component
        /// </summary>
        public int extraJumps { get; set; } = 0;
        #endregion

        /// <summary>
        /// Helper for the ExtraJumps component, which handles extra jumps for the player
        /// </summary>
        /// <param name="modName"></param>
        /// <param name="featureName"></param>
        /// <param name="extraJumps"></param>
        public JumpHelper(string modName, string featureName, int extraJumps)
        {
            this.modName = modName;
            this.featureName = featureName;
            this.extraJumps = extraJumps;
        }

        /// <summary>
        /// Creates or updates the ExtraJumps component
        /// </summary>
        public void Start()
        {
            ExtraJumps component = HeroController.instance.gameObject.GetOrAddComponent<ExtraJumps>();
            if (!component.ModList.ContainsKey(modName))
            {
                component.ModList.Add(modName, new System.Collections.Generic.Dictionary<string, int>());
            }

            if (!component.ModList[modName].ContainsKey(featureName))
            {
                component.ModList[modName].Add(featureName, extraJumps);
            }

            component.ModList[modName][featureName] = extraJumps;
        }

        /// <summary>
        /// Removes the the feature's extra jumps from the component
        /// </summary>
        public void Stop()
        {
            ExtraJumps component = HeroController.instance.gameObject.GetOrAddComponent<ExtraJumps>();
            if (!component.ModList.ContainsKey(modName))
            {
                component.ModList.Add(modName, new System.Collections.Generic.Dictionary<string, int>());
            }

            if (component.ModList[modName].ContainsKey(featureName))
            {
                component.ModList[modName].Remove(featureName);
            }
        }
    }
}