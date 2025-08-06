using DanielSteginkUtils.Helpers.Charms.Dung;

namespace DanielSteginkUtils.Helpers.Charms.Pets
{
    /// <summary>
    /// Umbrella helper that increases damage dealt by all pets: Grimmchild, Glowing Womb, Weaversong and Flukenest
    /// </summary>
    public class AllPetsHelper
    {
        #region Variables
        /// <summary>
        /// Grimmchild Utils
        /// </summary>
        private GrimmchildHelper grimmHelper;

        /// <summary>
        /// Glowing Womb Utils
        /// </summary>
        private HatchlingHelper hatchlingHelper;

        /// <summary>
        /// Weaversong Utils
        /// </summary>
        private WeaverlingHelper weaverHelper;

        /// <summary>
        /// Flukenest Utils
        /// </summary>
        private FlukeHelper flukeHelper;

        /// <summary>
        /// Flukenest + Dung Defender Utils
        /// </summary>
        private DungFlukeHelper dungFlukeHelper;
        #endregion

        /// <summary>
        /// Helper for adjusting the damage dealt by pets
        /// </summary>
        /// <param name="modName"></param>
        /// <param name="featureName"></param>
        /// <param name="damageModifier"></param>
        /// <param name="performLogging"></param>
        public AllPetsHelper(string modName, string featureName, float damageModifier, bool performLogging = false)
        {
            grimmHelper = new GrimmchildHelper(damageModifier, 1f, performLogging);
            hatchlingHelper = new HatchlingHelper(damageModifier, performLogging);
            weaverHelper = new WeaverlingHelper(damageModifier, performLogging);
            flukeHelper = new FlukeHelper(damageModifier, performLogging);
            dungFlukeHelper = new DungFlukeHelper(modName, featureName, 1 / damageModifier, performLogging);
        }

        /// <summary>
        /// Starts each of the child helpers
        /// </summary>
        public void Start()
        {
            grimmHelper.Start();
            hatchlingHelper.Start();
            weaverHelper.Start();
            flukeHelper.Start();
            dungFlukeHelper.Start();
        }

        /// <summary>
        /// Stops each of the child helpers
        /// </summary>
        public void Stop()
        {
            grimmHelper.Stop();
            hatchlingHelper.Start();
            weaverHelper.Stop();
            flukeHelper.Stop();
            dungFlukeHelper.Stop();
        }
    }
}
