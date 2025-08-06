using DanielSteginkUtils.Components.Dung;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DanielSteginkUtils.Helpers.Charms.Dung
{
    /// <summary>
    /// Dung Flukes don't store damage like regular flukes. Instead, they explode creating a 
    /// custom dung cloud similar to the ones created by Defender's Crest.
    /// 
    /// My attempts to modify the FSMs creating these clouds have ended in failure, so instead 
    /// I made this helper that uses coroutines to find fluke clouds as they are made, then 
    /// apply the modifier provided in the constructor.
    /// </summary>
    public class DungFlukeHelper : CustomBuffHelper<BuffDungFluke, float>
    {
        /// <summary>
        /// Helper for adjusting the damage rate of clouds created by Dung Flukes
        /// </summary>
        /// <param name="modName"></param>
        /// <param name="featureName"></param>
        /// <param name="modifier"></param>
        /// <param name="performLogging"></param>
        public DungFlukeHelper(string modName, string featureName, float modifier, bool performLogging = false) : 
                base(modName, featureName, modifier, performLogging) { }

        /// <summary>
        /// Gets all Dung Fluke clouds
        /// </summary>
        /// <returns></returns>
        public override List<GameObject> GetObjects()
        {
            List<GameObject> objects = base.GetObjects();
            return objects.Where(x => x.name.StartsWith("Knight Dung Cloud"))
                            .ToList();
        }

        /// <summary>
        /// Gets the damage rate of a cloud
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public override float GetCurrentValue(GameObject gameObject)
        {
            DamageEffectTicker damageEffectTicker = gameObject.GetComponent<DamageEffectTicker>();
            return damageEffectTicker.damageInterval;
        }

        /// <summary>
        /// Applies the modifiers to the damage rate
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="modsApplied"></param>
        public override void ApplyBuff(GameObject gameObject, BuffDungFluke modsApplied)
        {
            DamageEffectTicker damageEffectTicker = gameObject.GetComponent<DamageEffectTicker>();
            damageEffectTicker.SetDamageInterval(modsApplied.GetModdedValue());
        }
    }
}