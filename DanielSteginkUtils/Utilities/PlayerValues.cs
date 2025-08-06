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
            if (PlayerData.instance.GetBool("soulLimited"))
            {
                return 66;
            }

            // While doing a Godhome pantheon with the SOUL binding, max MP is reduced even more
            if (BossSequenceController.BoundSoul)
            {
                return 33;
            }

            // Max MP the player has by default
            int regularMax = PlayerData.instance.GetInt("maxMP");

            // Max MP from the Soul Vessels
            int vesselMax = PlayerData.instance.GetInt("MPReserveMax");

            return regularMax + vesselMax;
        }

        /// <summary>
        /// The player's current total SOUL
        /// </summary>
        /// <returns></returns>
        public static int CurrentSoul()
        {
            int currentMp = PlayerData.instance.GetInt("MPCharge");
            int reserveMp = PlayerData.instance.GetInt("MPReserve");
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
            return Math.Min(amount, PlayerData.instance.GetInt("MPCharge"));
        }

        /// <summary>
        /// Whether or not the player has all of Cornifer's maps
        /// </summary>
        /// <returns></returns>
        public static bool BoughtAllMaps()
        {
            return PlayerData.instance.GetBool("mapCrossroads") &&
                    PlayerData.instance.GetBool("mapGreenpath") &&
                    PlayerData.instance.GetBool("mapFogCanyon") &&
                    PlayerData.instance.GetBool("mapRoyalGardens") &&
                    PlayerData.instance.GetBool("mapFungalWastes") &&
                    PlayerData.instance.GetBool("mapCity") &&
                    PlayerData.instance.GetBool("mapWaterways") &&
                    PlayerData.instance.GetBool("mapMines") &&
                    PlayerData.instance.GetBool("mapDeepnest") &&
                    PlayerData.instance.GetBool("mapCliffs") &&
                    PlayerData.instance.GetBool("mapOutskirts") &&
                    PlayerData.instance.GetBool("mapRestingGrounds") &&
                    PlayerData.instance.GetBool("mapAbyss");
        }
    }
}