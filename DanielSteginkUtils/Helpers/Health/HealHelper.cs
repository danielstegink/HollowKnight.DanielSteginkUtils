using DanielSteginkUtils.Utilities;

namespace DanielSteginkUtils.Helpers.Health
{
    /// <summary>
    /// Helper for giving the player extra health when healing
    /// </summary>
    public class HealHelper
    {
        /// <summary>
        /// Likelihood of healing to occur
        /// </summary>
        public int healChance { get; set; } = 0;

        /// <summary>
        /// Whether or not to log results
        /// </summary>
        private bool performLogging { get; set; } = false;

        /// <summary>
        /// Helper for giving the player extra health when healing
        /// </summary>
        /// <param name="healChance"></param>
        /// <param name="performLogging"></param>
        public HealHelper(int healChance, bool performLogging = false)
        {
            this.healChance = healChance;
            this.performLogging = performLogging;
        }

        /// <summary>
        /// Applies hooks
        /// </summary>
        public void Start()
        {
            On.HeroController.AddHealth += IncreaseHealing;
        }

        /// <summary>
        /// Removes hooks
        /// </summary>
        public void Stop()
        {
            On.HeroController.AddHealth -= IncreaseHealing;
        }

        /// <summary>
        /// Has a chance to increase the health received
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        /// <param name="amount"></param>
        private void IncreaseHealing(On.HeroController.orig_AddHealth orig, HeroController self, int amount)
        {
            int random = UnityEngine.Random.Range(1, 101);
            if (performLogging)
            {
                Logging.Log("HealHelper", $"Random chance {random} versus {healChance}");
            }

            if (random <= healChance &&
                CustomCheck())
            {
                amount++;
            }

            orig(self, amount);
        }

        /// <summary>
        /// Determines if we should give extra health
        /// </summary>
        /// <returns></returns>
        public virtual bool CustomCheck()
        {
            return true;
        }
    }
}
