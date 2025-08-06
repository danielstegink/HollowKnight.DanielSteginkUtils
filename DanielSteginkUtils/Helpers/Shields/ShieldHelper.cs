using DanielSteginkUtils.Utilities;
using GlobalEnums;
using System;
using System.Collections;
using UnityEngine;

namespace DanielSteginkUtils.Helpers.Shields
{
    /// <summary>
    /// Template helper for giving the player a chance to ignore damage
    /// </summary>
    public abstract class ShieldHelper
    {
        /// <summary>
        /// If the player is currently immune to damage
        /// </summary>
        private bool isImmune = false;

        /// <summary>
        /// Whether or not we negate damage the first time the block is triggered
        /// </summary>
        public virtual bool blockFirstDamage { get; set; } = true;

        /// <summary>
        /// Hooks into the TakeDamage event so we can negate damage if needed
        /// </summary>
        public virtual void Start()
        {
            On.HeroController.TakeDamage += Block;
        }

        /// <summary>
        /// Unhooks from the TakeDamage event when we don't want to block damage anymore
        /// </summary>
        public virtual void Stop()
        {
            On.HeroController.TakeDamage -= Block;
        }

        /// <summary>
        /// Performs a check to see if we can block damage, and handles the blocking process
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        /// <param name="go"></param>
        /// <param name="damageSide"></param>
        /// <param name="damageAmount"></param>
        /// <param name="hazardType"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void Block(On.HeroController.orig_TakeDamage orig, HeroController self, GameObject go, CollisionSide damageSide, int damageAmount, int hazardType)
        {
            if (CanTakeDamage(damageAmount, hazardType))
            {
                if (isImmune)
                {
                    damageAmount = 0;
                }
                else if (CustomShieldCheck())
                {
                    if (blockFirstDamage)
                    {
                        damageAmount = 0;
                    }

                    GameManager.instance.StartCoroutine(Invincibility());
                }
            }

            orig(self, go, damageSide, damageAmount, hazardType);
        }

        /// <summary>
        /// Determines if the player can take damage
        /// </summary>
        /// <param name="damageAmount"></param>
        /// <param name="hazardType"></param>
        /// <returns></returns>
        public virtual bool CanTakeDamage(int damageAmount, int hazardType)
        {
            return Logic.CanTakeDamage(damageAmount, hazardType);
        }

        /// <summary>
        /// Placeholder for special logic determining if we can block the next attack
        /// </summary>
        /// <returns></returns>
        public abstract bool CustomShieldCheck();

        /// <summary>
        /// Provides imitation for the I-Frames that the player usually gets after being damaged
        /// </summary>
        /// <returns></returns>
        public IEnumerator Invincibility()
        {
            isImmune = true;

            yield return CustomEffects();

            isImmune = false;
        }

        /// <summary>
        /// Placeholder for special I-Frame effects unique to the calling class
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator CustomEffects()
        {
            yield return new WaitForSeconds(GetIFrames());
        }

        /// <summary>
        /// Gets the default length of I-Frames from the Hero Controller.
        /// This also accounts for it Stalwart Shell is equipped.
        /// </summary>
        /// <returns></returns>
        public float GetIFrames()
        {
            // Hero Controller handles I-Frames normally, so we can use its variables
            // Of course, we need to check for Stalwart Shell as well
            float iFramesLength = HeroController.instance.INVUL_TIME;
            if (PlayerData.instance.GetBool("equippedCharm_4"))
            {
                iFramesLength = HeroController.instance.INVUL_TIME_STAL;
            }

            return iFramesLength;
        }
    }
}