using DanielSteginkUtils.Components;
using DanielSteginkUtils.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DanielSteginkUtils.Helpers.Charms.Pets
{
    /// <summary>
    /// Aspid Aspect uses the ApplyExtraDamage event to deal damage to enemies.
    /// This doesn't have a means to trace it's source (that I've found), so I'm resorting to the CustomBuffHelper.
    /// </summary>
    public class AspidAspectHelper : CustomBuffHelper<BuffAspidAspect, int>
    {
        /// <summary>
        /// Helper for adjusting the damage dealt by Nickc01's Aspid Aspect
        /// </summary>
        /// <param name="modName"></param>
        /// <param name="featureName"></param>
        /// <param name="modifier"></param>
        /// <param name="performLogging"></param>
        public AspidAspectHelper(string modName, string featureName, float modifier, bool performLogging) :
            base(modName, featureName, modifier, performLogging) { }

        /// <summary>
        /// Aspid Aspect deals damage with modified versions of the Aspid Shot prefab by adding a custom component called AspidShotExtraDamager
        /// </summary>
        /// <returns></returns>
        public override List<GameObject> GetObjects()
        {
            List<GameObject> objects = base.GetObjects();
            List<GameObject> aspidShots = objects.Where(x => x.name.StartsWith("Aspid Shot - Damage All"))
                                                    .ToList();

            List<GameObject> friendlyShots = new List<GameObject>();
            foreach (GameObject shot in aspidShots)
            {
                Component friendlyAspidComponent = shot.GetComponentsInChildren<Component>()
                                                        .Where(x => x.GetType().Name.Equals("AspidShotExtraDamager"))
                                                        .FirstOrDefault();
                if (friendlyAspidComponent != default)
                {
                    friendlyShots.Add(shot);
                }
            }

            return friendlyShots;
        }

        /// <summary>
        /// Replaces the AdditionalDamage field with the modded value
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="modsApplied"></param>
        /// <exception cref="ArgumentException"></exception>
        public override void ApplyBuff(GameObject gameObject, BuffAspidAspect modsApplied)
        {
            Component damager = gameObject.GetComponentsInChildren<Component>()
                                                            .Where(x => x.GetType().Name.Equals("AspidShotExtraDamager"))
                                                            .FirstOrDefault();
            if (damager != default)
            {
                ClassIntegrations.SetProperty<Component>(damager, "AdditionalDamage", modsApplied.GetModdedValue());
                if (performLogging)
                {
                    Logging.Log("AspidAspectHelper", $"Damage buffed from {modsApplied.BaseValue} to {modsApplied.GetModdedValue()}");
                }
            }
            else
            {
                throw new ArgumentException("Unable to set damage value for AspidAspectHelper.");
            }
        }

        /// <summary>
        /// Gets the current damage value from the AspidShotExtraDamager component
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public override int GetCurrentValue(GameObject gameObject)
        {
            Component damager = gameObject.GetComponentsInChildren<Component>()
                                                            .Where(x => x.GetType().Name.Equals("AspidShotExtraDamager"))
                                                            .FirstOrDefault();
            if (damager != default)
            {
                return ClassIntegrations.GetProperty<Component, int>(damager, "AdditionalDamage");
            }
            else
            {
                throw new ArgumentException("Unable to get damage value for AspidAspectHelper.");
            }
        }
    }
}
