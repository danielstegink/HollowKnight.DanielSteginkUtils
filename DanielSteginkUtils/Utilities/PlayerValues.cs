using System;

namespace DanielSteginkUtils.Utilities
{
    /// <summary>
    /// Library for getting player data
    /// </summary>
    public static class PlayerValues
    {
        /// <summary>
        /// The player's current maximum SOUL
        /// </summary>
        /// <param name="getCurrentMax">If true, consider restrictions like injuries and Godhome bindings</param>
        /// <returns></returns>
        public static int MaxSoul(bool getCurrentMax)
        {
            // If the player has died, their max MP will be reduced
            if (PlayerData.instance.soulLimited)
            {
                return 66;
            }

            // While doing a Godhome pantheon with the SOUL binding, max MP is reduced even more
            if (BossSequenceController.BoundSoul)
            {
                return 33;
            }

            // Max MP the player has by default
            int regularMax = PlayerData.instance.maxMP;

            // Max MP from the Soul Vessels
            int vesselMax = PlayerData.instance.MPReserveMax;

            return regularMax + vesselMax;
        }

        /// <summary>
        /// The player's current total SOUL
        /// </summary>
        /// <returns></returns>
        public static int CurrentSoul()
        {
            int currentMp = PlayerData.instance.MPCharge;
            int reserveMp = PlayerData.instance.MPReserve;
            return currentMp + reserveMp;
        }

        /// <summary>
        /// The amount of SOUL that can be spent on a new spell
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static int SoulToSpend(int amount)
        {
            // The player can only spend the SOUL in their big orb on spells
            // Soul in the Soul Vessels is considered reserve and cannot be used
            return Math.Min(amount, PlayerData.instance.MPCharge);
        }

        /// <summary>
        /// Whether or not the player has all of Cornifer's maps
        /// </summary>
        /// <returns></returns>
        public static bool BoughtAllMaps()
        {
            return PlayerData.instance.mapCrossroads &&
                    PlayerData.instance.mapGreenpath &&
                    PlayerData.instance.mapFogCanyon &&
                    PlayerData.instance.mapRoyalGardens &&
                    PlayerData.instance.mapFungalWastes &&
                    PlayerData.instance.mapCity &&
                    PlayerData.instance.mapWaterways &&
                    PlayerData.instance.mapMines &&
                    PlayerData.instance.mapDeepnest &&
                    PlayerData.instance.mapCliffs &&
                    PlayerData.instance.mapOutskirts &&
                    PlayerData.instance.mapRestingGrounds &&
                    PlayerData.instance.mapAbyss;
        }
    }
}