using DanielSteginkUtils.Utilities;
using GlobalEnums;
using System;
using UnityEngine;

namespace DanielSteginkUtils.Helpers.Charms
{
    /// <summary>
    /// Helper for firing a Grubberfly's Elegy beam attack
    /// </summary>
    public class ElegyBeamAttacker
    {
        /// <summary>
        /// The chance out of 100 that a beam will fire
        /// </summary>
        public int beamChance { get; set; } = 0;

        /// <summary>
        /// Whether or not to log results
        /// </summary>
        private bool performLogging { get; set; } = false;

        /// <summary>
        /// Helper for firing a Grubberfly's Elegy beam attack
        /// </summary>
        /// <param name="beamChance"></param>
        /// <param name="performLogging"></param>
        public ElegyBeamAttacker(int beamChance, bool performLogging = false)
        {
            this.beamChance = beamChance;
            this.performLogging = performLogging;
        }

        /// <summary>
        /// Applies hooks
        /// </summary>
        public void Start()
        {
            On.HeroController.Attack += FireBeam;
        }

        /// <summary>
        /// Removes hooks
        /// </summary>
        public void Stop()
        {
            On.HeroController.Attack -= FireBeam;
        }

        /// <summary>
        /// Has a chance to fire a Grubberfly's Elegy beam attack
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        /// <param name="attackDir"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void FireBeam(On.HeroController.orig_Attack orig, HeroController self, AttackDirection attackDir)
        {
            orig(self, attackDir);

            int random = UnityEngine.Random.Range(1, 101);
            if (performLogging)
            {
                Logging.Log("ElegyBeamAttacker", $"Random chance {random} versus {beamChance}");
            }

            if (random <= beamChance)
            {
                GameObject grubberFlyBeam = default;
                float markOfPrideModifier = ClassIntegrations.GetField<HeroController, float>(HeroController.instance, "MANTIS_CHARM_SCALE");
                switch (attackDir)
                {
                    case AttackDirection.normal:
                        if (self.transform.localScale.x < 0f)
                        {
                            grubberFlyBeam = self.grubberFlyBeamPrefabR.Spawn(self.transform.position);
                        }
                        else
                        {
                            grubberFlyBeam = self.grubberFlyBeamPrefabL.Spawn(self.transform.position);
                        }
                        grubberFlyBeam.transform.SetScaleY(1f);

                        break;
                    case AttackDirection.upward:
                        grubberFlyBeam = self.grubberFlyBeamPrefabU.Spawn(self.transform.position);
                        grubberFlyBeam.transform.SetScaleY(self.transform.localScale.x);
                        grubberFlyBeam.transform.localEulerAngles = new Vector3(0f, 0f, 270f);

                        break;
                    case AttackDirection.downward:
                        grubberFlyBeam = self.grubberFlyBeamPrefabD.Spawn(self.transform.position);
                        grubberFlyBeam.transform.SetScaleY(self.transform.localScale.x);
                        grubberFlyBeam.transform.localEulerAngles = new Vector3(0f, 0f, 90f);

                        break;
                }

                if (PlayerData.instance.GetBool("equippedCharm_13"))
                {
                    grubberFlyBeam.transform.SetScaleY(grubberFlyBeam.transform.localScale.y * markOfPrideModifier);
                }
            }
        }
    }
}