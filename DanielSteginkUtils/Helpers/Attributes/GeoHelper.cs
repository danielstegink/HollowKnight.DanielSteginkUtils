using DanielSteginkUtils.Utilities;

namespace DanielSteginkUtils.Helpers.Attributes
{
    /// <summary>
    /// Helper for adjusting how much Geo the player gets
    /// </summary>
    public class GeoHelper
    {
        #region Variables
        /// <summary>
        /// How much to increase the geo gained by
        /// </summary>
        public float modifier { get; set; } = 1f;

        /// <summary>
        /// Whether or not to log results
        /// </summary>
        private bool performLogging { get; set; } = false;
        #endregion

        /// <summary>
        /// Helper for adjusting how much Geo the player gets
        /// </summary>
        /// <param name="modifier"></param>
        /// <param name="performLogging"></param>
        public GeoHelper(float modifier, bool performLogging = false)
        {
            this.modifier = modifier;
            this.performLogging = performLogging;
        }

        /// <summary>
        /// Applies hooks
        /// </summary>
        public void Start()
        {
            On.HeroController.AddGeo += AddGeo;
        }

        /// <summary>
        /// Removes hooks
        /// </summary>
        public void Stop()
        {
            On.HeroController.AddGeo -= AddGeo;
        }

        /// <summary>
        /// Adjusts the geo gained from any source
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        /// <param name="amount"></param>
        private void AddGeo(On.HeroController.orig_AddGeo orig, HeroController self, int amount)
        {
            int moddedGeo = Calculations.GetModdedInt(amount, modifier);

            orig(self, moddedGeo);
            if (performLogging)
            {
                Logging.Log("GeoHelper", $"Geo increased from {amount} to {moddedGeo}");
            }
        }
    }
}
