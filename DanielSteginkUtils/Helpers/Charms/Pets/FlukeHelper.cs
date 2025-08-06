using DanielSteginkUtils.Utilities;
using Modding;
using UnityEngine;

namespace DanielSteginkUtils.Helpers.Charms.Pets
{
    /// <summary>
    /// Helper for adjusting the damage dealt by Flukenest
    /// </summary>
    public class FlukeHelper
    {
        /// <summary>
        /// Adjusts the damage of the Spell Fluke
        /// </summary>
        private float modifier { get; set; } = 1f;

        /// <summary>
        /// Whether or not to log results
        /// </summary>
        private bool performLogging { get; set; } = false;

        /// <summary>
        /// Helper for adjusting the damage dealt by Flukenest (doesn't include Dung Flukes)
        /// </summary>
        /// <param name="modifier"></param>
        /// <param name="performLogging"></param>
        public FlukeHelper(float modifier, bool performLogging = false)
        {
            this.modifier = modifier;
            this.performLogging = performLogging;
        }
        
        /// <summary>
        /// Applies hooks
        /// </summary>
        public void Start()
        {
            ModHooks.ObjectPoolSpawnHook += BuffDamage;
        }

        /// <summary>
        /// Removes hooks
        /// </summary>
        public void Stop()
        {
            ModHooks.ObjectPoolSpawnHook -= BuffDamage;
        }

        /// <summary>
        /// Adjusts the damage of Spell Flukes
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        private GameObject BuffDamage(GameObject gameObject)
        {
            if (gameObject.name.StartsWith("Spell Fluke") &&
                gameObject.name.Contains("Clone")) // We can avoid double-buffing by targetting the clones
            {
                // Dung Flukes have to be handled separately
                if (!gameObject.name.Contains("Dung"))
                {
                    SpellFluke fluke = gameObject.GetComponent<SpellFluke>();

                    // Fluke damage is stored in a private variable, so we need to
                    //  get the field the variable is stored in
                    int baseDamage = ClassIntegrations.GetField<SpellFluke, int>(fluke, "damage");
                    int moddedDamage = Calculations.GetModdedInt(baseDamage, modifier);

                    ClassIntegrations.SetField(fluke, "damage", moddedDamage);
                    if (performLogging)
                    {
                        Logging.Log("FlukeHelper", $"Fluke damage increased from {baseDamage} to {moddedDamage}");
                    }
                }
            }

            return gameObject;
        }
    }
}
