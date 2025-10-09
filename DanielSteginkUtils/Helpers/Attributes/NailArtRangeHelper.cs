using DanielSteginkUtils.Utilities;
using System.Linq;
using UnityEngine;

namespace DanielSteginkUtils.Helpers.Attributes
{
    /// <summary>
    /// Nail Arts take the form of game objects with awkward inheritance issues that 
    /// make adjusting their size difficult, hence this template helper
    /// </summary>
    public abstract class NailArtRangeHelper
    {
        #region Variables
        /// <summary>
        /// Name of the calling mod
        /// </summary>
        public string modName { get; set; } = "";

        /// <summary>
        /// Name of the calling feature
        /// </summary>
        public string featureName { get; set; } = "";

        /// <summary>
        /// Modifier used to adjust the property of the given object
        /// </summary>
        public float modifier { get; set; } = 1f;

        /// <summary>
        /// Whether or not to log results
        /// </summary>
        public bool performLogging { get; set; } = false;

        /// <summary>
        /// Tracks if the buff has been applied
        /// </summary>
        private bool buffApplied { get; set; } = false;
        #endregion

        /// <summary>
        /// Nail Arts take the form of game objects with awkward inheritance issues that 
        /// make adjusting their size difficult, hence this helper
        /// </summary>
        /// <param name="modName"></param>
        /// <param name="featureName"></param>
        /// <param name="modifier"></param>
        /// <param name="performLogging"></param>
        public NailArtRangeHelper(string modName, string featureName, float modifier, bool performLogging)
        {
            this.modName = modName;
            this.featureName = featureName;
            this.modifier = modifier;
            this.performLogging = performLogging;
        }

        /// <summary>
        /// Nail Arts are best modified when their objects are activated in the FSM, and reset shortly after
        /// </summary>
        public void Start()
        {
            On.HutongGames.PlayMaker.FsmState.OnEnter += BuffNailArt;
        }

        /// <summary>
        /// Removes the hooks from Start
        /// </summary>
        public void Stop()
        {
            On.HutongGames.PlayMaker.FsmState.OnEnter -= BuffNailArt;
        }

        /// <summary>
        /// Custom logging for the helper
        /// </summary>
        /// <param name="message"></param>
        private void Log(string message)
        {
            Logging.Log($"{modName} - {featureName}", message);
        }

        /// <summary>
        /// When the FSM activates the Nail Art objects, we will apply our modifier to make them bigger or smaller
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        private void BuffNailArt(On.HutongGames.PlayMaker.FsmState.orig_OnEnter orig, HutongGames.PlayMaker.FsmState self)
        {
            orig(self);

            if (self.Fsm.Name.Equals("Nail Arts"))
            {
                string[] objectNames = new string[]
                {
                    "Cyclone Slash",
                    "Great Slash Effect",
                    "Dash Slash"
                };

                // If we enter one of the "activate Nail Art object" steps, apply the buff
                string[] stateNames = new string[]
                {
                    "Activate Slash",
                    "G Slash",
                    "Dash Slash",
                };
                if (stateNames.Contains(self.Name) && 
                    !buffApplied)
                {
                    foreach (string name in objectNames)
                    {
                        ApplyBuff(self, name, true);
                    }

                    buffApplied = true;
                }

                // If we enter the "reset to normal" step, remove the buff
                // We also need to reset if the animation has been canceled, such as when damage is taken
                if (buffApplied && 
                    (self.Name.Equals("Regain Control") ||
                        self.Name.Equals("Cancel All")))
                {
                    foreach (string name in objectNames)
                    {
                        ApplyBuff(self, name, false);
                    }

                    buffApplied = false;
                }
            }
        }

        /// <summary>
        /// Applies or resets the buff on the given Nail Art variable
        /// </summary>
        /// <param name="self"></param>
        /// <param name="nailArtName"></param>
        /// <param name="applyBuff"></param>
        private void ApplyBuff(HutongGames.PlayMaker.FsmState self, string nailArtName, bool applyBuff)
        {
            GameObject gameObject = self.Fsm.Variables.GetFsmGameObject(nailArtName).Value;

            if (applyBuff)
            {
                self.Fsm.Variables.GetFsmGameObject(nailArtName).Value.transform.localScale *= modifier;
                if (performLogging)
                {
                    Log($"Game object {gameObject.name} buffed by {modifier} to {gameObject.transform.localScale}");
                }
            }
            else
            {
                self.Fsm.Variables.GetFsmGameObject(nailArtName).Value.transform.localScale /= modifier;
                if (performLogging)
                {
                    Log($"Game object {gameObject.name} reset by {modifier} to {gameObject.transform.localScale}");
                }
            }
        }
    }
}