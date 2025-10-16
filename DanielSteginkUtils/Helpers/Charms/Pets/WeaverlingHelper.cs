using DanielSteginkUtils.Utilities;

namespace DanielSteginkUtils.Helpers.Charms.Pets
{
    /// <summary>
    /// Helper for adjusting the damage of weaverlings created by Weaversong 
    /// </summary>
    public class WeaverlingHelper
    {
        #region Variables
        /// <summary>
        /// Adjusts the damage
        /// </summary>
        private float damageModifier { get; set; } = 1f;

        /// <summary>
        /// Whether or not to log results
        /// </summary>
        private bool performLogging { get; set; } = false;
        #endregion

        /// <summary>
        /// Helper for adjusting the damage of weaverlings created by Weaversong 
        /// </summary>
        /// <param name="damageModifier"></param>
        /// <param name="performLogging"></param>
        public WeaverlingHelper(float damageModifier, bool performLogging = false)
        {
            this.damageModifier = damageModifier;
            this.performLogging = performLogging;
        }

        /// <summary>
        /// Applies hooks for hatchling damage
        /// </summary>
        public void Start()
        {
            On.HutongGames.PlayMaker.Actions.IntOperator.OnEnter += BuffWeaverlings;
        }

        /// <summary>
        /// Removes hooks
        /// </summary>
        public void Stop()
        {
            On.HutongGames.PlayMaker.Actions.IntOperator.OnEnter -= BuffWeaverlings;
        }

        /// <summary>
        /// Weaverlings, like Grimmballs, store their damage in an Enemy Damager object
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        private void BuffWeaverlings(On.HutongGames.PlayMaker.Actions.IntOperator.orig_OnEnter orig, HutongGames.PlayMaker.Actions.IntOperator self)
        {
            if (self.Fsm.Name.Equals("Attack") &&
                self.Fsm.GameObject.name.Equals("Enemy Damager") &&
                self.Fsm.GameObject.transform.parent.gameObject.name.Contains("Weaverling") &&
                self.State.Name.Equals("Hit"))
            {
                int baseDamage = self.Fsm.GetFsmInt("Damage").Value;
                self.Fsm.GetFsmInt("Damage").Value = Calculations.GetModdedInt(baseDamage, damageModifier);
                if (performLogging)
                {
                    Logging.Log("WeaverlingHelper", $"Weaverling damage increased from {baseDamage} to {self.Fsm.GetFsmInt("Damage").Value}");
                }

                orig(self);
                self.Fsm.GetFsmInt("Damage").Value = baseDamage;
            }
            else
            {
                orig(self);
            }
        }
    }
}