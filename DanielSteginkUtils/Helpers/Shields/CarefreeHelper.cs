using DanielSteginkUtils.Utilities;
using System;
using System.Collections;
using UnityEngine;

namespace DanielSteginkUtils.Helpers.Shields
{
    /// <summary>
    /// Helper for adding an additional chance of Carefree Melody triggering
    /// </summary>
    public class CarefreeHelper : ShieldHelper
    {
        /// <summary>
        /// Chance of the shield triggering during an attack
        /// </summary>
        private int     shieldChance { get; set; } = 0;

        /// <summary>
        /// Whether or not to send logs
        /// </summary>
        private bool performLogging { get; set; } = false;

        /// <summary>
        /// Helper for adding an additional chance of Carefree Melody triggering.
        /// Using NotchCosts.ShieldChancePerNotch and Calculations.GetSecondMelodyShield to get shieldChance is recommended.
        /// </summary>
        /// <param name="shieldChance"></param>
        /// <param name="performLogging"></param>
        public CarefreeHelper(int shieldChance, bool performLogging = false)
        {
            this.shieldChance = shieldChance;
            this.performLogging = performLogging;
        }

        /// <summary>
        /// Carefree has an integer that represents the percent chance of blocking an attack
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override bool CustomShieldCheck()
        {
            int random = UnityEngine.Random.Range(1, 101);
            if (performLogging)
            {
                Logging.Log("CarefreeHelper", $"Random chance {random} versus {shieldChance}");
            }

            return random <= shieldChance;
        }

        /// <summary>
        /// In addition to the 1.3 seconds of I-Frames, CM has a special VFX that triggers
        /// </summary>
        /// <returns></returns>
        public override IEnumerator CustomEffects()
        {
            GameObject carefreeShield = HeroController.instance.carefreeShield;
            if (carefreeShield != null )
            {
                carefreeShield.SetActive(true);
            }

            return base.CustomEffects();
        }
    }
}
