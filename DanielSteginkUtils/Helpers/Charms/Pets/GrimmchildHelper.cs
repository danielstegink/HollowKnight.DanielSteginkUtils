using DanielSteginkUtils.Utilities;
using HutongGames.PlayMaker.Actions;
using System.Linq;

namespace DanielSteginkUtils.Helpers.Charms.Pets
{
    /// <summary>
    /// Helper for adjusting the damage and attack rate of Grimmchild
    /// </summary>
    public class GrimmchildHelper
    {
        #region Variables
        /// <summary>
        /// Adjusts the damage of the Grimmchild's fireball
        /// </summary>
        private float damageModifier { get; set; } = 1f;

        /// <summary>
        /// Adjusts the Grimmchild's attack speed
        /// </summary>
        private float attackModifier { get; set; } = 1f;

        /// <summary>
        /// Whether or not to log results
        /// </summary>
        private bool performLogging { get; set; } = false;
        #endregion

        /// <summary>
        /// Helper for adjusting the damage and attack rate of Grimmchild
        /// </summary>
        /// <param name="damageModifier"></param>
        /// <param name="attackModifier"></param>
        /// <param name="performLogging"></param>
        public GrimmchildHelper(float damageModifier, float attackModifier, bool performLogging = false)
        {
            this.damageModifier = damageModifier;
            this.attackModifier = attackModifier;
            this.performLogging = performLogging;
        }

        /// <summary>
        /// Applies hooks
        /// </summary>
        public void Start()
        {
            On.HutongGames.PlayMaker.Actions.IntOperator.OnEnter += BuffDamage;
            On.HutongGames.PlayMaker.Actions.SetFloatValue.OnEnter += BuffSpeed;
            On.HutongGames.PlayMaker.Actions.RandomFloat.OnEnter += BuffRandomSpeed;
        }

        /// <summary>
        /// Removes hooks
        /// </summary>
        public void Stop()
        {
            On.HutongGames.PlayMaker.Actions.IntOperator.OnEnter -= BuffDamage;
            On.HutongGames.PlayMaker.Actions.SetFloatValue.OnEnter -= BuffSpeed;
            On.HutongGames.PlayMaker.Actions.RandomFloat.OnEnter -= BuffRandomSpeed;
        }

        /// <summary>
        /// Stores the damage states of the Grimmchild FSM
        /// </summary>
        private string[] damageStates = new string[]
        {
            "Level 2",
            "Level 3",
            "Level 4"
        };


        /// <summary>
        /// Grimmballs store their damage in an Enemy Damager object which has an Attack FSM that directly subtracts from enemy HP
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        private void BuffDamage(On.HutongGames.PlayMaker.Actions.IntOperator.orig_OnEnter orig, HutongGames.PlayMaker.Actions.IntOperator self)
        {
            int baseDamage = 0;
            if (self.Fsm.Name.Equals("Attack") &&
                self.Fsm.GameObject.name.Equals("Enemy Damager") &&
                self.Fsm.GameObject.transform.parent.gameObject.name.Contains("Grimmball") &&
                self.State.Name.Equals("Hit"))
            {
                baseDamage = self.Fsm.GetFsmInt("Damage").Value;
                self.Fsm.GetFsmInt("Damage").Value = Calculations.GetModdedInt(baseDamage, damageModifier);
                if (performLogging)
                {
                    Logging.Log("GrimmchildHelper", $"Grimmchild damage increased from {baseDamage} to {self.Fsm.GetFsmInt("Damage").Value}");
                }
            }

            orig(self);

            // Reset damage afterwards; Weaverling FSMs don't reset like Grimmballs
            if (baseDamage > 0)
            {
                self.Fsm.GetFsmInt("Damage").Value = baseDamage;
            }
        }

        /// <summary>
        /// Adjusts the attack speed of the Grimmchild
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        private void BuffSpeed(On.HutongGames.PlayMaker.Actions.SetFloatValue.orig_OnEnter orig, SetFloatValue self)
        {
            if (self.Fsm.GameObject.name.Equals("Grimmchild(Clone)") && 
                self.Fsm.Name.Equals("Control") && 
                (self.State.Name.Equals("Pause") || 
                    self.State.Name.Equals("Spawn")) &&
                attackModifier != 1)
            {
                float baseValue = self.floatValue.Value;
                self.floatValue.Value *= attackModifier;
                if (performLogging)
                {
                    Logging.Log("GrimmchildHelper", $"Attack speed reduced from {baseValue} to {self.floatValue.Value}");
                }
            }

            orig(self);
        }

        /// <summary>
        /// Adjusts the attack speed of the Grimmchild (props to Charm Changer cuz I had no idea this was a thing)
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        private void BuffRandomSpeed(On.HutongGames.PlayMaker.Actions.RandomFloat.orig_OnEnter orig, RandomFloat self)
        {
            if (self.Fsm.GameObject.name.Equals("Grimmchild(Clone)") &&
                self.Fsm.Name.Equals("Control") && 
                self.State.Name.Equals("Antic") &&
                attackModifier != 1)
            {
                float baseMinValue = self.min.Value;
                float baseMaxValue = self.min.Value;

                self.min.Value *= attackModifier;
                self.max.Value *= attackModifier;
                if (performLogging)
                {
                    Logging.Log("GrimmchildHelper", $"Random speed reduced from ({baseMinValue} - {baseMaxValue}) to ({self.min.Value} - {self.max.Value})");
                }
            }

            orig(self);
        }
    }
}
