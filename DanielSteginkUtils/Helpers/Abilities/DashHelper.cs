using DanielSteginkUtils.Utilities;
using HKMirror.Reflection.SingletonClasses;

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
            On.HeroController.HeroDash += BuffDashCooldowns;
        }

        /// <summary>
        /// Resets the cooldowns
        /// </summary>
        public virtual void Stop()
        {
            On.HeroController.HeroDash -= BuffDashCooldowns;
        }

        /// <summary>
        /// Various mods (most famously Charm Changer) overwrite the dash cooldowns manually,
        /// so in order to modify those values short of overwriting them, we have to modify 
        /// the timer those values are used for directly
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        private void BuffDashCooldowns(On.HeroController.orig_HeroDash orig, HeroController self)
        {
            // Let the controller do its thing so Charm Changer takes effect and we don't get overwritten
            orig(self);

            float dashCooldown = HeroControllerR.dashCooldownTimer;
            float shadowCooldown = HeroControllerR.shadowDashTimer;

            HeroControllerR.dashCooldownTimer *= dashModifier;
            if (performLogging)
            {
                Logging.Log("DashHelper", $"Dash cooldown: {dashCooldown} -> {HeroControllerR.dashCooldownTimer}");
            }

            if (self.cState.shadowDashing)
            {
                HeroControllerR.shadowDashTimer *= darkDashModifier;
                if (performLogging)
                {
                    Logging.Log("DashHelper", $"Shade Dash cooldown: {shadowCooldown} -> {HeroControllerR.shadowDashTimer}");
                }
            }
        }
    }
}