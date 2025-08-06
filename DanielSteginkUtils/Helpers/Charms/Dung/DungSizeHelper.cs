using DanielSteginkUtils.Components.Dung;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DanielSteginkUtils.Helpers.Charms.Dung
{
    /// <summary>
    /// Dung Clouds from Defender's Crest get spawned and recycled multiple times, 
    /// making it hard to track which ones have been buffed.
    /// 
    /// Additionally, Pale Court straight up destroys the original object pool and 
    /// creates its own clouds, so it's just easier to search for the clouds periodically, 
    /// buff them, and then add a component so we know a given cloud has been modified.
    /// </summary>
    public class DungSizeHelper : CustomBuffHelper<BuffDungSize, Vector3>
    {
        /// <summary>
        /// Helper for adjusting the size of clouds created with Defender's Crest
        /// </summary>
        /// <param name="modName"></param>
        /// <param name="featureName"></param>
        /// <param name="modifier"></param>
        /// <param name="performLogging"></param>
        public DungSizeHelper(string modName, string featureName, float modifier, bool performLogging = false) : 
                base(modName, featureName, modifier, performLogging) { }

        /// <summary>
        /// Gets clouds created by Defender's Crest
        /// </summary>
        /// <returns></returns>
        public override List<GameObject> GetObjects()
        {
            List<GameObject> objects = base.GetObjects();
            return objects.Where(x => x.name.StartsWith("Knight Dung Trail(Clone)"))
                            .ToList();
        }

        /// <summary>
        /// Gets the current size of the cloud
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
        public override void ApplyBuff(GameObject gameObject, BuffDungSize modsApplied)
        {
            gameObject.transform.localScale = modsApplied.GetModdedValue();
        }
    }
}