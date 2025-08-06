using DanielSteginkUtils.Components.Dung;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DanielSteginkUtils.Helpers.Charms.Spores
{
    /// <summary>
    /// Spore Shroom clouds, like Dung clouds, are recycled in a confusing way so that 
    /// buffing a cloud only once is tricky. So here we are.
    /// </summary>
    public class SporeSizeHelper : CustomBuffHelper<BuffSporeSize, Vector3>
    {
        /// <summary>
        /// Helper for adjusting the size of clouds created by Spore Shroom
        /// </summary>
        /// <param name="modName"></param>
        /// <param name="featureName"></param>
        /// <param name="modifier"></param>
        /// <param name="performLogging"></param>
        public SporeSizeHelper(string modName, string featureName, float modifier, bool performLogging = false) : 
                base(modName, featureName, modifier, performLogging) { }

        /// <summary>
        /// Gets spore clouds
        /// </summary>
        /// <returns></returns>
        public override List<GameObject> GetObjects()
        {
            List<GameObject> objects = base.GetObjects();
            return objects.Where(x => x.name.StartsWith("Knight Spore Cloud(Clone)"))
                            .ToList();
        }

        /// <summary>
        /// Gets current size of the cloud
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public override Vector3 GetCurrentValue(GameObject gameObject)
        {
            return gameObject.transform.localScale;
        }

        /// <summary>
        /// Applies the modifiers to the size dimensions
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="modsApplied"></param>
        public override void ApplyBuff(GameObject gameObject, BuffSporeSize modsApplied)
        {
            gameObject.transform.localScale = modsApplied.GetModdedValue();
        }
    }
}