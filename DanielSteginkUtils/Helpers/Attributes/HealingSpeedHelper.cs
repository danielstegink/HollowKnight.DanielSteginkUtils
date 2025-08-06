using DanielSteginkUtils.Utilities;

namespace DanielSteginkUtils.Helpers.Attributes
{
    /// <summary>
    /// Helper for adjusting the player's healing speed
    /// </summary>
    public class HealingSpeedHelper
    {
        #region Variables
        /// <summary>
        /// How much to adjust the player's healing speed by
        /// </summary>
        public float modifier { get; set; } = 1f;

        /// <summary>
        /// Whether or not to send logs
        /// </summary>
        public bool performLogging { get; set; } = false;
        #endregion

        /// <summary>
        /// Helper for adjusting the player's healing speed
        /// </summary>
        /// <param name="modifier"></param>
        /// <param name="performLogging"></param>
        public HealingSpeedHelper(float modifier, bool performLogging = false)
        {
            this.modifier = modifier;
            this.performLogging = performLogging;
        }

        /// <summary>
        /// Applies hooks
        /// </summary>
        public void Start()
        {
            On.HeroController.StartMPDrain += SpeedHealing;
        }

        /// <summary>
        /// Removes hooks
        /// </summary>
        public void Stop()
        {
            On.HeroController.StartMPDrain -= SpeedHealing;
        }

        /// <summary>
        /// Adjusts healing speed
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        /// <param name="time"></param>
        private void SpeedHealing(On.HeroController.orig_StartMPDrain orig, HeroController self, float time)
        {
            time *= modifier;
            if (performLogging)
            {
                Logging.Log("HealingSpeedHelper", $"Healing speed set to {time}");
            }

            orig(self, time);
        }
    }
}
