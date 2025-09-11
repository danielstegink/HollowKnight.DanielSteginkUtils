using DanielSteginkUtils.Utilities;

namespace DanielSteginkUtils.Helpers.Attributes
{
    /// <summary>
    /// Helper for adjusting the number of hits required to stagger a boss
    /// </summary>
    public class StaggerHelper
    {
        #region Variables
        /// <summary>
        /// How many points to reduce the enemy's max stagger requirement by
        /// </summary>
        public int maxModifier { get; set; } = 0;

        /// <summary>
        /// How many points to reduce the enemy's combo stagger requirement by
        /// </summary>
        public int comboModifier { get; set; } = 0;

        /// <summary>
        /// Whether or not to perform logging
        /// </summary>
        public bool performLogging { get; set; } = false;
        #endregion

        /// <summary>
        /// Helper for adjusting the number of hits required to stagger a boss
        /// </summary>
        /// <param name="maxModifier"></param>
        /// <param name="comboModifier"></param>
        /// <param name="performLogging"></param>
        public StaggerHelper(int maxModifier, int comboModifier = 0, bool performLogging = false)
        {
            this.maxModifier = maxModifier;
            this.comboModifier = comboModifier;
            this.performLogging = performLogging;
        }

        /// <summary>
        /// Applies hooks
        /// </summary>
        public void Start()
        {
            On.HutongGames.PlayMaker.Actions.IntCompare.OnEnter += ExtraStagger;
        }

        /// <summary>
        /// Removes hooks
        /// </summary>
        public void Stop()
        {
            On.HutongGames.PlayMaker.Actions.IntCompare.OnEnter += ExtraStagger;
        }

        /// <summary>
        /// Reduces the number of hits required to stagger a boss
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        private void ExtraStagger(On.HutongGames.PlayMaker.Actions.IntCompare.orig_OnEnter orig, HutongGames.PlayMaker.Actions.IntCompare self)
        {
            // Caches modified value for easy reset
            int originalValue = self.integer2.Value;

            // Make sure this is a Stun FSM
            if (self.Fsm.Name.Equals("Stun") ||
                self.Fsm.Name.Equals("Stun Control"))
            {
                // Make sure this is one of the stun check states
                if (self.State.Name.Equals("Max Check") ||
                    self.State.Name.Equals("Continue Combo"))
                {
                    self.integer2.Value -= maxModifier;
                    if (self.integer2.Value < 1)
                    {
                        self.integer2.Value = 1;
                    }

                    if (performLogging)
                    {
                        Logging.Log("StaggerHelper", $"Max stun reduced by {maxModifier} to {self.integer2.Value}");
                    }
                }
                else if (self.State.Name.Equals("In Combo"))
                {
                    self.integer2.Value -= comboModifier;
                    if (self.integer2.Value < 1)
                    {
                        self.integer2.Value = 1;
                    }

                    if (performLogging)
                    {
                        Logging.Log("StaggerHelper", $"Combo stun reduced by {comboModifier} to {self.integer2.Value}");
                    }
                }
            }

            orig(self);

            // Reset the stun value so it doesn't stack
            self.integer2.Value = originalValue;
            if (performLogging)
            {
                Logging.Log("StaggerHelper", $"Stun reset to {self.integer2.Value}");
            }
        }
    }
}