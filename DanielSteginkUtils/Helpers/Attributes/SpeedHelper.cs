using DanielSteginkUtils.Utilities;
using System.Collections;
using UnityEngine;

namespace DanielSteginkUtils.Helpers.Attributes
{
    /// <summary>
    /// Helper for adjusting player's movement speed
    /// </summary>
    public class SpeedHelper
    {
        #region Variables
        /// <summary>
        /// How much to adjust the player's movement speed by
        /// </summary>
        public float speedModifier { get; set; } = 1f;

        /// <summary>
        /// How much to adjust the player's Shape of Unn speed by
        /// </summary>
        public float unnModifier { get; set; } = 1f;

        /// <summary>
        /// Whether or not to send logs
        /// </summary>
        public bool performLogging { get; set; } = false;
        #endregion

        /// <summary>
        /// Helper for adjusting player's movement speed
        /// </summary>
        /// <param name="speedModifier"></param>
        /// <param name="unnModifier"></param>
        /// <param name="performLogging"></param>
        public SpeedHelper(float speedModifier, float unnModifier = 1f, bool performLogging = false)
        {
            this.speedModifier = speedModifier;
            this.unnModifier = unnModifier;
            this.performLogging = performLogging;
        }

        /// <summary>
        /// Applies hooks and starts coroutines
        /// </summary>
        public void Start()
        {
            On.HeroController.Move += SpeedBoost;

            isUnnActive = true;
            GameManager.instance.StartCoroutine(UnnBoost());
        }

        /// <summary>
        /// Removes hooks and stops coroutines
        /// </summary>
        public void Stop()
        {
            On.HeroController.Move -= SpeedBoost;

            isUnnActive = false;
        }

        /// <summary>
        /// Adjusts the player's movement speed
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        /// <param name="move_direction"></param>
        private void SpeedBoost(On.HeroController.orig_Move orig, HeroController self, float move_direction)
        {
            move_direction *= speedModifier;
            if (performLogging)
            {
                Logging.Log("SpeedHelper", $"Movement speed increased to {move_direction}");
            }

            orig(self, move_direction);
        }

        /// <summary>
        /// Tracks if the SOU speed boost should be active
        /// </summary>
        private bool isUnnActive = false;

        /// <summary>
        /// Tracks if we need to apply (or re-apply) the SOU boost
        /// </summary>
        private bool applyUnn = true;

        /// <summary>
        /// SOU affects the 2D body instead of using the move event, so we use a coroutine to adjust it
        /// </summary>
        /// <returns></returns>
        private IEnumerator UnnBoost()
        {
            // If the glyph has been unequipped, we can stop
            while (isUnnActive)
            {
                // We only need to apply it if we're healing and SOU is equipped
                if (PlayerData.instance.equippedCharm_28 &&
                    HeroController.instance.cState.focusing)
                {
                    Rigidbody2D rb2d = ClassIntegrations.GetField<HeroController, Rigidbody2D>(HeroController.instance, "rb2d");
                    if (rb2d.velocity.x == 0) // If we've stopped, the movement has reset and we need to re-apply the buff
                    {
                        applyUnn = true;
                    }
                    else if (applyUnn)
                    {
                        rb2d.velocity = new Vector2(rb2d.velocity.x * unnModifier, rb2d.velocity.y);
                        if (performLogging)
                        {
                            Logging.Log("SpeedHelper", $"Unn speed set to {rb2d.velocity}");
                        }

                        applyUnn = false; // Once we've applied the buff, we don't want to keep applying it
                    }
                }
                else // If we stopped focusing or SOU is unequipped, that is a reset
                {
                    applyUnn = true;
                }

                yield return new WaitForSeconds(Time.deltaTime);
            }

            // Once the coroutine stops, that is also a reset
            applyUnn = true;
        }

    }
}
