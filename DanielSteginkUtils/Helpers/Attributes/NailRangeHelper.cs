using System;
using UnityEngine;

namespace DanielSteginkUtils.Helpers.Attributes
{
    /// <summary>
    /// Template helper for adjusting the range of nail attacks
    /// </summary>
    public abstract class NailRangeHelper
    {
        /// <summary>
        /// Regular animation used by basic attacks and long nail is
        /// smaller than the one used by Mark of Pride.
        /// 
        /// So even if an attack is bigger, if Mark of Pride isn't
        /// equipped it may end up looking smaller.
        /// </summary>
        public bool forceMantisAnim { get; set; } = false;

        /// <summary>
        /// Applies hooks
        /// </summary>
        public void Start()
        {
            On.NailSlash.StartSlash += IncreaseNailRange;
        }

        /// <summary>
        /// Removes hooks
        /// </summary>
        public void Stop()
        {
            On.NailSlash.StartSlash -= IncreaseNailRange;
        }

        /// <summary>
        /// Increases range of basic nail attacks
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void IncreaseNailRange(On.NailSlash.orig_StartSlash orig, NailSlash self)
        {
            // Get the default scale
            Vector3 startingScale = self.scale;

            // Forces MOP animation
            if (forceMantisAnim)
            {
                self.SetMantis(true);

                // If MOP isn't equipped, we need to adjust the vector to offset the boost NailSlash gives by default
                if (!PlayerData.instance.equippedCharm_13)
                {
                    float resetModifier = 1f;
                    if (PlayerData.instance.equippedCharm_18)
                    {
                        // If Longnail is equipped, we need to offset the 40% boost to be 15% instead
                        resetModifier = 1.15f / 1.4f;
                    }
                    else
                    {
                        // If neither is equipped, we need to offset the 25% boost to be 0%
                        resetModifier = 1f / 1.25f;
                    }

                    self.scale = new Vector3(startingScale.x * resetModifier, startingScale.y * resetModifier);
                }
            }

            // Increase the scale
            if (CustomCheck())
            {
                float modifier = GetModifier();
                Vector3 newScale = new Vector3(self.scale.x * modifier, self.scale.y * modifier);
                self.scale = newScale;
            }

            // Perform the nail slash
            orig(self);

            // Reset the scale so the effects don't stack
            self.scale = startingScale;
        }

        /// <summary>
        /// Validates that we can increase nail range
        /// </summary>
        /// <returns></returns>
        public abstract bool CustomCheck();

        /// <summary>
        /// Modifier for adjusting nail range
        /// </summary>
        /// <returns></returns>
        public abstract float GetModifier();
    }
}