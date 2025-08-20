using DanielSteginkUtils.Utilities;

namespace DanielSteginkUtils.Helpers.Abilities
{
    /// <summary>
    /// Helper for modifying dash cooldowns
    /// </summary>
    public class DashHelper
    {
        #region Variables
        /// <summary>
        /// Modifier for Dash cooldown
        /// </summary>
        public float dashModifier { get; set; } = 1f;

        /// <summary>
        /// Modifier for Shade Dash cooldown
        /// </summary>
        public float darkDashModifier { get; set; } = 1f;

        /// <summary>
        /// Whether or not to log changes
        /// </summary>
        public bool performLogging { get; set; } = true;

        /// <summary>
        /// Tracks if the buff has been applied
        /// </summary>
        private bool isActive { get; set; } = false;
        #endregion

        /// <summary>
        /// Helper for modifying dash cooldowns
        /// </summary>
        /// <param name="dashModifier"></param>
        /// <param name="darkDashModifier"></param>
        /// <param name="performLogging"></param>
        public DashHelper(float dashModifier, float darkDashModifier = 1f, bool performLogging = false)
        {
            this.dashModifier = dashModifier;
            this.darkDashModifier = darkDashModifier;
            this.performLogging = performLogging;
        }

        /// <summary>
        /// Applies the modifiers to the cooldowns
        /// </summary>
        public virtual void Start()
        {
            if (!isActive)
            {
                HeroController.instance.DASH_COOLDOWN *= dashModifier;
                HeroController.instance.DASH_COOLDOWN_CH *= dashModifier;
                HeroController.instance.SHADOW_DASH_COOLDOWN *= darkDashModifier;
                isActive = true;

                if (performLogging)
                {
                    Logging.Log("DashHelper", "Dash cooldowns reduced");
                    Logging.Log("DashHelper", $"Dash cooldown: {HeroController.instance.DASH_COOLDOWN}");
                    Logging.Log("DashHelper", $"Dashmaster cooldown: {HeroController.instance.DASH_COOLDOWN_CH}");
                    Logging.Log("DashHelper", $"Shade Dash cooldown: {HeroController.instance.SHADOW_DASH_COOLDOWN}");
                }
            }
        }

        /// <summary>
        /// Resets the cooldowns
        /// </summary>
        public virtual void Stop()
        {
            if (isActive)
            {
                HeroController.instance.DASH_COOLDOWN /= dashModifier;
                HeroController.instance.DASH_COOLDOWN_CH /= dashModifier;
                HeroController.instance.SHADOW_DASH_COOLDOWN /= darkDashModifier;
                isActive = false;

                if (performLogging)
                {
                    Logging.Log("DashHelper", "Dash cooldowns reset");
                    Logging.Log("DashHelper", $"Dash cooldown: {HeroController.instance.DASH_COOLDOWN}");
                    Logging.Log("DashHelper", $"Dashmaster cooldown: {HeroController.instance.DASH_COOLDOWN_CH}");
                    Logging.Log("DashHelper", $"Shade Dash cooldown: {HeroController.instance.SHADOW_DASH_COOLDOWN}");
                }
            }
        }
    }
}