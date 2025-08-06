using DanielSteginkUtils.Components.Dung;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DanielSteginkUtils.Helpers.Charms.Shroom
{
    /// <summary>
    /// Spore Shroom clouds, like Dung clouds, are recycled in a confusing way so that 
    /// buffing a cloud only once is tricky. So here we 
    /// </summary>
    public class SporeDamageHelper : CustomBuffHelper<BuffDungDamage, float>
    {
        /// <summary>
        /// Helper for adjusting the damage of clouds created by Spore Shroom
        /// </summary>
        /// <param name="modName"></param>
        /// <param name="featureName"></param>
        /// <param name="modifier"></param>
        /// <param name="performLogging"></param>
        public SporeDamageHelper(string modName, string featureName, float modifier, bool performLogging = false) : 
                base(modName, featureName, modifier, performLogging) { }

        /// <summary>
        /// Gets all spore clouds
        /// </summary>
        /// <returns></returns>
        public override List<GameObject> GetObjects()
        {
            List<GameObject> objects = base.GetObjects();
            return objects.Where(x => x.name.StartsWith("Knight Spore Cloud(Clone)"))
                            .ToList();
        }

        /// <summary>
        /// Gets damage rate of the cloud
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
        public override void ApplyBuff(GameObject gameObject, BuffDungDamage modsApplied)
        {
            DamageEffectTicker damageEffectTicker = gameObject.GetComponent<DamageEffectTicker>();
            damageEffectTicker.SetDamageInterval(modsApplied.GetModdedValue());
        }
    }
}