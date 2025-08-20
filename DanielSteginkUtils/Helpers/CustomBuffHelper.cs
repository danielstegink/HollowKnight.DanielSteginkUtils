using DanielSteginkUtils.Components;
using DanielSteginkUtils.Utilities;
using Modding.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DanielSteginkUtils.Helpers
{
    /// <summary>
    /// Template class for applying a custom buff to specific game objects where regular hooks aren't feasible
    /// </summary>
    public abstract class CustomBuffHelper<M, T> where M : ModBuffs<T>
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
        /// Whether or not the coroutine should be running
        /// </summary>
        private bool isActive { get; set; } = false;
        #endregion

        /// <summary>
        /// Custom helper for modifying items dynamically where hooks can't help.
        /// 
        /// This helper adds a custom component called ModBuffs to a given object to 
        /// track which mods and which features have been applied to the object, 
        /// then uses them to modify the given property.
        /// </summary>
        /// <param name="modName"></param>
        /// <param name="featureName"></param>
        /// <param name="modifier"></param>
        /// <param name="performLogging"></param>
        public CustomBuffHelper(string modName, string featureName, float modifier, bool performLogging)
        {
            this.modName = modName;
            this.featureName = featureName;
            this.modifier = modifier;
            this.performLogging = performLogging;
        }

        /// <summary>
        /// Starts the coroutine that applies the modifier
        /// </summary>
        public void Start()
        {
            if (performLogging)
            {
                Log("Starting run");
            }

            isActive = true;
            GameManager.instance.StartCoroutine(ApplyBuff());
        }

        /// <summary>
        /// Stops the coroutine and removes the modifier
        /// </summary>
        public void Stop()
        {
            if (performLogging)
            {
                Log("Stopping");
            }

            isActive = false;
            RemoveBuff();
        }

        /// <summary>
        /// Loops constantly to find eligible game objects and modify the given property
        /// </summary>
        /// <returns></returns>
        private IEnumerator ApplyBuff()
        {
            while (isActive)
            {
                AdjustObjects();

                yield return new WaitForSeconds(Time.deltaTime);
            }
        }

        /// <summary>
        /// Removes the buff applied by the given mod/feature from all game objects
        /// </summary>
        private void RemoveBuff()
        {
            AdjustObjects(false);
        }

        /// <summary>
        /// Finds all eligible game objects and adjusts the buff
        /// </summary>
        /// <param name="applyBuff">If true, apply the buff. If false, remove it.</param>
        private void AdjustObjects(bool applyBuff = true)
        {
            // Get a list of all eligible game objects
            List<GameObject> gameObjects = GetObjects();
            foreach (GameObject gameObject in gameObjects)
            {
                // Get or add a ModBuffs component
                M modsApplied = gameObject.GetOrAddComponent<M>();
                if (performLogging)
                {
                    Log($"Mods applied: {string.Join("|", modsApplied.ModList.Keys)}, " +
                        $"Base value: {modsApplied.BaseValue}");
                }

                // Get the current value
                T currentValue = GetCurrentValue(gameObject);

                // If we are modding for the first time, the base value will be default, so
                // we need to store the current value for future reference
                if (modsApplied.BaseValue.Equals(default(T)))
                {
                    modsApplied.BaseValue = currentValue;
                }

                // If current value doesn't match the modded value, the game object may have reset
                T moddedValue = modsApplied.GetModdedValue();
                if (!moddedValue.Equals(currentValue))
                {
                    // If it has reset, we have nothing to worry about, as we will re-apply the buffs at the end

                    // However, if the base value has changed, we need to do a full reset so the Mods can re-apply themselves
                    if (!modsApplied.BaseValue.Equals(currentValue))
                    {
                        modsApplied.BaseValue = currentValue;
                        modsApplied.ModList = new Dictionary<string, Dictionary<string, float>>();
                    }
                }

                // With the base value established, we can modify our feature
                if (!modsApplied.ModList.ContainsKey(modName))
                {
                    modsApplied.ModList.Add(modName, new Dictionary<string, float>());
                }

                if (applyBuff && 
                    !modsApplied.ModList[modName].ContainsKey(featureName))
                {
                    modsApplied.ModList[modName].Add(featureName, modifier);
                    if (performLogging)
                    {
                        Log($"{featureName} adjusts value by {modifier}");
                    }
                }
                else if (!applyBuff && 
                            modsApplied.ModList[modName].ContainsKey(featureName))
                {
                    modsApplied.ModList[modName].Remove(featureName);
                    if (performLogging)
                    {
                        Log($"{featureName} no longer adjusts value by {modifier}");
                    }
                }

                // Finally, set the game object to have the modded property value
                ApplyBuff(gameObject, modsApplied);
            }
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
        /// Placeholder for getting eligible game objects
        /// </summary>
        /// <returns></returns>
        public virtual List<GameObject> GetObjects()
        {
            return Object.FindObjectsOfType<GameObject>()
                                            .ToList();
        }

        /// <summary>
        /// Placeholder for getting the current value of the modified property
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public abstract T GetCurrentValue(GameObject gameObject);

        /// <summary>
        /// Placeholder for applying the buff to the game object
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="modsApplied"></param>
        public abstract void ApplyBuff(GameObject gameObject, M modsApplied);
    }
}
