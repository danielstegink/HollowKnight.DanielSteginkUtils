using DanielSteginkUtils.Utilities;
using GlobalEnums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DanielSteginkUtils.Components
{
    /// <summary>
    /// Handles the hooks and behavior for giving the player extra jumps
    /// </summary>
    public class ExtraJumps : MonoBehaviour
    {
        /// <summary>
        /// Stores how many extra jumps the player has received from various mods / features
        /// </summary>
        public Dictionary<string, Dictionary<string, int>> ModList = new Dictionary<string, Dictionary<string, int>>();

        /// <summary>
        /// Counts how many extra jumps the player has performed
        /// </summary>
        private int jumpCounter { get; set; } = 0;

        /// <summary>
        /// Triggered when the component is enabled
        /// </summary>
        public void OnEnable()
        {
            On.HeroController.CanDoubleJump += AllowExtraJump;
            On.HeroController.DoDoubleJump += DoExtraJump;
            On.HeroController.BackOnGround += ResetJump;
        }

        /// <summary>
        /// Triggered when the component is disabled
        /// </summary>
        public void OnDisable()
        {
            On.HeroController.CanDoubleJump -= AllowExtraJump;
            On.HeroController.DoDoubleJump -= DoExtraJump;
            On.HeroController.BackOnGround -= ResetJump;
        }

        /// <summary>
        /// Checks if the player can perform an extra jump
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        /// <returns></returns>
        private bool AllowExtraJump(On.HeroController.orig_CanDoubleJump orig, HeroController self)
        {
            // If able to double-jump, do that first
            if (orig(self))
            {
                return true;
            }

            // Double jump has a number of factors it checks to see if the player can double-jump
            // The only one we really overwrite is whether they've double-jumped
            ActorStates[] landedStates = new ActorStates[] { ActorStates.no_input, ActorStates.hard_landing, ActorStates.dash_landing };
            bool inAir = !self.inAcid &&
                            !self.cState.dashing &&
                            !self.cState.wallSliding &&
                            !self.cState.backDashing &&
                            !self.cState.attacking &&
                            !self.cState.bouncing &&
                            !self.cState.shroomBouncing &&
                            !self.cState.onGround;
            return PlayerData.instance.hasDoubleJump &&
                    !self.controlReqlinquished &&
                    !landedStates.Contains(self.hero_state) &&
                    inAir &&
                    jumpCounter < GetTotalJumps();
        }

        /// <summary>
        /// We need to add a special case to the double jump so it supports extra jumps
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        private void DoExtraJump(On.HeroController.orig_DoDoubleJump orig, HeroController self)
        {
            // If we are not double-jumping, we need to reset the wings for graphics purposes
            //      and note that we've used an extra jump
            bool isDoubleJumping = !ClassIntegrations.GetField<HeroController, bool>(self, "doubleJumped");
            if (!isDoubleJumping)
            {
                self.dJumpWingsPrefab.SetActive(false);
                self.dJumpFlashPrefab.SetActive(false);
                jumpCounter++;
            }

            orig(self);
        }

        /// <summary>
        /// Once we've landed on the ground, reset the jump counter
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        private void ResetJump(On.HeroController.orig_BackOnGround orig, HeroController self)
        {
            jumpCounter = 0;
            orig(self);
        }

        /// <summary>
        /// Gets the total number of extra jumps the player has
        /// </summary>
        /// <returns></returns>
        public int GetTotalJumps()
        {
            int totalJumps = 0;
            foreach (string mod in ModList.Keys)
            {
                foreach (string feature in ModList[mod].Keys)
                {
                    totalJumps += ModList[mod][feature];
                }
            }

            return totalJumps;
        }
    }
}
