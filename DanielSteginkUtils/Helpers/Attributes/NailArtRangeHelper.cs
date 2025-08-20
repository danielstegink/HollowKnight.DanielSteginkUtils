using DanielSteginkUtils.Components;
using DanielSteginkUtils.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DanielSteginkUtils.Helpers.Attributes
{
    /// <summary>
    /// Nail Arts take the form of game objects with awkward inheritance issues that 
    /// make adjusting their size difficult, hence this template helper
    /// </summary>
    public abstract class NailArtRangeHelper : CustomBuffHelper<BuffNailArtSize, Vector3>
    {
        /// <summary>
        /// Nail Arts take the form of game objects with awkward inheritance issues that 
        /// make adjusting their size difficult, hence this helper
        /// </summary>
        /// <param name="modName"></param>
        /// <param name="featureName"></param>
        /// <param name="modifier"></param>
        /// <param name="performLogging"></param>
        public NailArtRangeHelper(string modName, string featureName, float modifier, bool performLogging) : 
            base(modName, featureName, modifier, performLogging) { }

        /// <summary>
        /// Gets Nail Art attack objects
        /// </summary>
        /// <returns></returns>
        public override List<GameObject> GetObjects()
        {
            List<GameObject> objects = base.GetObjects();
            return objects.Where(x => Logic.IsNailArt(x.name))
                            .ToList();
        }

        /// <summary>
        /// Applies the size modifier
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="modsApplied"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void ApplyBuff(GameObject gameObject, BuffNailArtSize modsApplied)
        {
            gameObject.transform.localScale = modsApplied.GetModdedValue();
        }

        /// <summary>
        /// Gets the base size of the object
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public override Vector3 GetCurrentValue(GameObject gameObject)
        {
            return gameObject.transform.localScale;
        }
    }
}