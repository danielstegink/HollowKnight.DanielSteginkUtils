using System;
using System.Linq;

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

        /// <summary>
        /// Gets the number of Mask shards collected
        /// </summary>
        /// <returns></returns>
        public static int MaskShardsFound()
        {
            int shards = 0;

            // 4 are bought from Sly
            if (PlayerData.instance.slyShellFrag1)
            {
                shards++;
            }

            if (PlayerData.instance.slyShellFrag2)
            {
                shards++;
            }

            if (PlayerData.instance.slyShellFrag3)
            {
                shards++;
            }

            if (PlayerData.instance.slyShellFrag4)
            {
                shards++;
            }

            // 1 is reward by the Seer
            if (PlayerData.instance.dreamReward7)
            {
                shards++;
            }

            // 11 are found in the wild
            shards += SceneData.instance.persistentBoolItems
                                        .Where(x => x.id.Equals("Heart Piece"))
                                        .Count();

            return shards;
        }

        /// <summary>
        /// Gets the number of Vessel Fragments found
        /// </summary>
        /// <returns></returns>
        public static int VesselFragmentsFound()
        {
            int fragments = 0;

            // 2 are bought from Sly
            if (PlayerData.instance.slyVesselFrag1)
            {
                fragments++;
            }

            if (PlayerData.instance.slyVesselFrag2)
            {
                fragments++;
            }

            // 1 is given by the Seer
            if (PlayerData.instance.dreamReward5)
            {
                fragments++;
            }

            // 1 is found in the Stag Nest
            if (PlayerData.instance.vesselFragStagNest)
            {
                fragments++;
            }

            // 5 are found in the wild
            fragments += SceneData.instance.persistentBoolItems
                                        .Where(x => x.id.Equals("Vessel Fragment"))
                                        .Count();

            return fragments;
        }
    }
}