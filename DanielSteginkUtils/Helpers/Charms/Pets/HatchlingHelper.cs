using DanielSteginkUtils.Utilities;

namespace DanielSteginkUtils.Helpers.Charms.Pets
{
    /// <summary>
    /// Helper for adjusting the damage of hatchlings created by Glowing Womb
    /// </summary>
    public class HatchlingHelper
    {
        #region Variables
        /// <summary>
        /// Adjusts the damage
        /// </summary>
        private float damageModifier { get; set; } = 1f;

        /// <summary>
        /// Whether or not to log results
        /// </summary>
        private bool performLogging { get; set; } = false;
        #endregion

        /// <summary>
        /// Helper for adjusting the damage of hatchlings created by Glowing Womb
        /// </summary>
        /// <param name="damageModifier"></param>
        /// <param name="performLogging"></param>
        public HatchlingHelper(float damageModifier, bool performLogging = false)
        {
            this.damageModifier = damageModifier;
            this.performLogging = performLogging;
        }

        /// <summary>
        /// Applies hooks for hatchling damage
        /// </summary>
        public void Start()
        {
            On.HealthManager.Hit += BuffHatchlings;
        }

        /// <summary>
        /// Removes hooks
        /// </summary>
        public void Stop()
        {
            On.HealthManager.Hit -= BuffHatchlings;
        }

        /// <summary>
        /// Hatchling damage is dealt by the Damager child object, so we hook into that
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        /// <param name="hitInstance"></param>
        private void BuffHatchlings(On.HealthManager.orig_Hit orig, HealthManager self, HitInstance hitInstance)
        {
            string parentName = "";
            if (hitInstance.Source.gameObject.name.Equals("Damager"))
            {
                // Check if the parent of the Damager object is a Knight Hatchling
                try
                {
                    parentName = hitInstance.Source.gameObject.transform.parent.name;
                }
                catch { }

                if (parentName.Contains("Knight Hatchling"))
                {
                    int baseDamage = hitInstance.DamageDealt;
                    hitInstance.DamageDealt = Calculations.GetModdedInt(baseDamage, damageModifier);
                    if (performLogging)
                    {
                        Logging.Log("HatchlingHelper", $"Hatchling damage increased from {baseDamage} to {hitInstance.DamageDealt}");
                    }
                }
            }

            orig(self, hitInstance);
        }
    }
}
