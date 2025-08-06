using GlobalEnums;
using System.Collections.Generic;

namespace DanielSteginkUtils.Utilities
{
    /// <summary>
    /// Library for logic I've used unrelated to calculations or charm notches
    /// </summary>
    public static class Logic
    {
        /// <summary>
        /// List of the object names of the regular nail attacks
        /// </summary>
        public static List<string> nailAttackNames = new List<string>()
        {
            "Slash",
            "AltSlash",
            "UpSlash",
            "DownSlash",
        };

        /// <summary>
        /// List of the object names of the Nail Art attacks
        /// </summary>
        public static List<string> nailArtNames = new List<string>()
        {
            "Cyclone Slash",
            "Great Slash",
            "Dash Slash",
            "Hit L",
            "Hit R"
        };

        /// <summary>
        /// Determines if an attack is a nail attack
        /// </summary>
        /// <param name="hitInstance"></param>
        /// <param name="allowNailArts"></param>
        /// <param name="allowGrubberfly"></param>
        /// <returns></returns>
        public static bool IsNailAttack(HitInstance hitInstance, bool allowNailArts = true, bool allowGrubberfly = true)
        {
            if (nailAttackNames.Contains(hitInstance.Source.name))
            {
                return true;
            }

            if (allowNailArts &&
                nailArtNames.Contains(hitInstance.Source.name))
            {
                return true;
            }

            if (allowGrubberfly &&
                hitInstance.Source.name.Contains("Grubberfly"))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines if the player can take damage
        /// </summary>
        /// <param name="damageAmount"></param>
        /// <param name="hazardType"></param>
        /// <returns></returns>
        public static bool CanTakeDamage(int damageAmount, int hazardType)
        {
            // Damage is calculated by the HeroController's TakeDamage method
            // First, run the CanTakeDamage check from the HeroController
            bool canTakeDamage = ClassIntegrations.CallFunction<HeroController, bool>(HeroController.instance, "CanTakeDamage", null);
            if (canTakeDamage)
            {
                // There is an additional check for I-Frames and shadow dashing
                if (hazardType == 1)
                {
                    if (HeroController.instance.damageMode == DamageMode.HAZARD_ONLY ||
                        HeroController.instance.cState.shadowDashing ||
                        HeroController.instance.parryInvulnTimer > 0f)
                    {
                        canTakeDamage = false;
                    }
                }
                else if (HeroController.instance.cState.invulnerable ||
                            PlayerData.instance.isInvincible)
                {
                    canTakeDamage = false;
                }
                else if (damageAmount <= 0)
                {
                    canTakeDamage = false;
                }
            }

            return canTakeDamage;
        }
    }
}
