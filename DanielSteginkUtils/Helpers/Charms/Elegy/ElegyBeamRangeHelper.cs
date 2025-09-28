using DanielSteginkUtils.Utilities;
using HutongGames.PlayMaker;
using SFCore.Utils;
using UnityEngine;

namespace DanielSteginkUtils.Helpers.Charms.Elegy
{
    /// <summary>
    /// Like Dung and Spore clouds, Elegy beam attacks are extremely finicky in terms of static properties.
    /// 
    /// Fortunately, in Elegy beam's case I can just hook into the FSM and tamper with the velocity.
    /// </summary>
    public abstract class ElegyBeamRangeHelper
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
        /// Whether or not to log results
        /// </summary>
        public bool performLogging { get; set; } = false;
        #endregion

        /// <summary>
        /// Elegy beams take the form of game objects with awkward inheritance issues that 
        /// make adjusting their range difficult, hence this helper
        /// </summary>
        /// <param name="modName"></param>
        /// <param name="featureName"></param>
        /// <param name="performLogging"></param>
        public ElegyBeamRangeHelper(string modName, string featureName, bool performLogging)
        {
            this.modName = modName;
            this.featureName = featureName;
            this.performLogging = performLogging;
        }

        /// <summary>
        /// Adjusting the size of Elegy beams doesn't affect their range very much, so I have to
        /// adjust their velocity instead
        /// </summary>
        public void Start()
        {
            //On.ObjectPool.Spawn_GameObject_Transform_Vector3_Quaternion += BuffBeam;
            On.HutongGames.PlayMaker.Actions.SetVelocity2d.OnEnter += BuffBeamSpeed;
        }

        /// <summary>
        /// Removes the hooks from Start
        /// </summary>
        public void Stop()
        {
            //On.ObjectPool.Spawn_GameObject_Transform_Vector3_Quaternion -= BuffBeam;
            On.HutongGames.PlayMaker.Actions.SetVelocity2d.OnEnter -= BuffBeamSpeed;
        }

        /// <summary>
        /// At first I tried modifying the velocity when the beam is created, but stacking became an issue and I abandoned the idea.
        /// I decided to leave this in here though, in case anyone wants to know how to hook into an Elegy beam's creation.
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="prefab"></param>
        /// <param name="parent"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        private GameObject BuffBeam(On.ObjectPool.orig_Spawn_GameObject_Transform_Vector3_Quaternion orig, GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
        {
            GameObject newObject = orig(prefab, parent, position, rotation);
            if (newObject.name.Contains("Grubberfly Beam"))
            {
                PlayMakerFSM fsm = newObject.LocateMyFSM("Control");
                FsmState init = fsm.GetState("Init");
                init.AddMethod(() =>
                {
                    string direction = GetDirection(newObject.name);
                    float modifier = GetModifier(direction);

                    Rigidbody2D rigidbody2D = newObject.GetComponent<Rigidbody2D>();
                    rigidbody2D.velocity *= modifier;

                    if (performLogging)
                    {
                        Log($"Beam {newObject.name}'s speed adjusted by {modifier} to {rigidbody2D.velocity}");
                    }
                });
            }

            return newObject;
        }

        /// <summary>
        /// Instead, we can hook into the SetVelocity2d step of the Init state of the FSM
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        private void BuffBeamSpeed(On.HutongGames.PlayMaker.Actions.SetVelocity2d.orig_OnEnter orig, HutongGames.PlayMaker.Actions.SetVelocity2d self)
        {
            orig(self);

            try
            {
                GameObject beamObject = self.Fsm.GameObject;
                if (self.Fsm.Name.Equals("Control") &&
                    beamObject.name.Contains("Grubberfly"))
                {
                    string direction = GetDirection(beamObject.name);
                    float modifier = GetModifier(direction);

                    Rigidbody2D rigidbody2D = beamObject.GetComponent<Rigidbody2D>();
                    rigidbody2D.velocity *= modifier;

                    if (performLogging)
                    {
                        Log($"Beam {beamObject.name}'s speed adjusted by {modifier} to {rigidbody2D.velocity}");
                    }
                }
            }
            catch
            {
                // If an FSM doesn't have a game object for whatever reason, we want to just move on
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
        /// Gets the modifier to apply to the beam. The base game gives slightly different bonuses to
        /// beam attacks based on the direction, so I'm leaving control of that in the modder's hands.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public abstract float GetModifier(string direction);

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