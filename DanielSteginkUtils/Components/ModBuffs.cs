using System.Collections.Generic;
using UnityEngine;

namespace DanielSteginkUtils.Components
{
    /// <summary>
    /// Custom component for tracking which mods/features are applying a buff to a given game object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ModBuffs<T> : MonoBehaviour
    {
        /// <summary>
        /// List of mods/features to apply to the object
        /// </summary>
        public Dictionary<string, Dictionary<string, float>> ModList = new Dictionary<string, Dictionary<string, float>>();

        /// <summary>
        /// The original value of the buffed property
        /// </summary>
        public T BaseValue = default;

        /// <summary>
        /// Gets the cumulative modifier of all the mods/features applied
        /// </summary>
        /// <returns></returns>
        public float GetModifier()
        {
            float modifier = 1f;
            foreach (string mod in ModList.Keys)
            {
                foreach (string feature in ModList[mod].Keys)
                {
                    modifier *= ModList[mod][feature];
                }
            }

            return modifier;
        }

        /// <summary>
        /// Applies modifiers to base value
        /// </summary>
        /// <returns></returns>
        public abstract T GetModdedValue();
    }
}