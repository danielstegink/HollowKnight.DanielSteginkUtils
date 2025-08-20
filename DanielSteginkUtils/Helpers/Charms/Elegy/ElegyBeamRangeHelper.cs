using DanielSteginkUtils.Components.Dung;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DanielSteginkUtils.Helpers.Charms.Elegy
{
    /// <summary>
    /// Like Dung and Spore clouds, Elegy beam attacks are extremely finicky in terms of static properties.
    /// 
    /// As such, I have to make helpers like this one which add custom components that track the original size
    /// and apply all mods' modifiers in a batch
    /// </summary>
    public abstract class ElegyBeamRangeHelper : CustomBuffHelper<BuffElegyRange, Vector3>
    {
        /// <summary>
        /// Helper for adjusting the range of beams created with Grubberfly's Elegy
        /// </summary>
        /// <param name="modName"></param>
        /// <param name="featureName"></param>
        /// <param name="modifier"></param>
        /// <param name="performLogging"></param>
        public ElegyBeamRangeHelper(string modName, string featureName, float modifier, bool performLogging = false) : 
            base(modName, featureName, modifier, performLogging) { }

        /// <summary>
        /// Gets beams created by Grubberfly's Elegy
        /// </summary>
        /// <returns></returns>
        public override List<GameObject> GetObjects()
        {
            List<GameObject> objects = base.GetObjects();
            List<GameObject> beamObjects = objects.Where(x => x.name.StartsWith("Grubberfly Beam"))
                                                    .ToList();

            // Unlike Dung and Spore clouds, Elegy beams can/will clone from each other and inherit active dimensions and components

            // So a buffed Elegy beam will clone into a new beam, giving it inflated data that gets treated as normal when it resets
            // the base value

            // To combat this, we will find the ones with buff component here in the GetObjects step and reset them to use their base value
            foreach (GameObject beam in beamObjects)
            {
                BuffElegyRange buffComponent = beam.GetComponent<BuffElegyRange>();
                if (buffComponent != null)
                {
                    beam.transform.localScale = buffComponent.BaseValue;
                }
            }

            return beamObjects;
        }

        /// <summary>
        /// Gets the current size of the beam
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public override Vector3 GetCurrentValue(GameObject gameObject)
        {
            return gameObject.transform.localScale;
        }

        /// <summary>
        /// Applies the modifiers to the size dimensions
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="modsApplied"></param>
        public override void ApplyBuff(GameObject gameObject, BuffElegyRange modsApplied)
        {
            string direction = GetDirection(gameObject.name);
            Vector3 baseValue = modsApplied.BaseValue;
            Vector3 moddedValue = modsApplied.GetModdedValue();

            // If a beam gets too big vertically, it collides with the floor and does nothing, 
            // so its important to only adjust the direction the beam is going
            if (direction.Equals("RIGHT") ||
                direction.Equals("LEFT"))
            {
                gameObject.transform.localScale = new Vector3(moddedValue.x, baseValue.y);
            }
            else
            {
                gameObject.transform.localScale = new Vector3(baseValue.x, moddedValue.y);
            }
        }

        /// <summary>
        /// Gets the direction the beam is going
        /// </summary>
        /// <param name="objectName"></param>
        /// <returns></returns>
        public string GetDirection(string objectName)
        {
            if (objectName.StartsWith("Grubberfly BeamR"))
            {
                return "RIGHT";
            }
            else if (objectName.StartsWith("Grubberfly BeamL"))
            {
                return "LEFT";
            }
            else if (objectName.StartsWith("Grubberfly BeamU"))
            {
                return "UP";
            }
            else if (objectName.StartsWith("Grubberfly BeamD"))
            {
                return "DOWN";
            }
            else
            {
                return "";
            }
        }
    }
}