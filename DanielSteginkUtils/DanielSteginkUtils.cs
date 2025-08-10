using System.Collections.Generic;
using Modding;
using UnityEngine;

namespace DanielSteginkUtils
{
    /// <summary>
    /// Custom Utilities mod I made to store logic, calculations and helper classes I've used across multiple mods
    /// </summary>
    public class DanielSteginkUtils : Mod
    {
        /// <summary>
        /// Static instance of this mod I use for logging purposes
        /// </summary>
        public static DanielSteginkUtils Instance;

        /// <summary>
        /// Version of the mod. 
        /// Hopefully this number doesn't change too often.
        /// </summary>
        /// <returns></returns>
        public override string GetVersion() => "1.1.0.0";

        /// <summary>
        /// Initialization step. Fairly useless here, but vital in most mods.
        /// </summary>
        /// <param name="preloadedObjects"></param>
        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects) 
        {
            Log("Initializing");

            Instance = this;

            Log("Initialized");
        }
    }
}