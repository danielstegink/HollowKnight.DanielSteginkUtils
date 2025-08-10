using SFCore;

namespace DanielSteginkUtils.Helpers.Charms.Templates
{
    /// <summary>
    /// Custom version of EasyCharmState for ExaltedCharm
    /// </summary>
    public class ExaltedCharmState : EasyCharmState
    {
        /// <summary>
        /// Tracks if the charm has been upgraded
        /// </summary>
        public bool IsUpgraded;
    }
}
