using SFCore;
using SFCore.Generics;
using System.Linq;

namespace DanielSteginkUtils.Helpers.Charms
{
    /// <summary>
    /// Helper for finding if a charm has been added by another mod
    /// </summary>
    public static class GetModCharmHelper
    {
        /// <summary>
        /// Checks charms added by mods to see if one has one of the given names. Defaults to -1.
        /// </summary>
        /// <param name="charmNames"></param>
        public static int GetCharmId(string[] charmNames)
        {
            int customCharmCount = FullSettingsMod<SFCoreSaveSettings, SFCoreGlobalSettings>.GlobalSettings.MaxCustomCharms;
            for (int i = 41; i <= 40 + customCharmCount; i++)
            {
                string charmName = Language.Language.Get($"CHARM_NAME_{i}", "UI");
                if (charmNames.Contains(charmName))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
