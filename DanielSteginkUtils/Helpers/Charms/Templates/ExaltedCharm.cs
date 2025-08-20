using Modding;
using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DanielSteginkUtils.Helpers.Charms.Templates
{
    /// <summary>
    /// Template class for custom charms that can be upgraded via the Exaltation mod
    /// </summary>
    public abstract class ExaltedCharm : TemplateCharm
    {
        /// <summary>
        /// Template class for custom charms that can be upgraded via the Exaltation mod
        /// </summary>
        /// <param name="modName"></param>
        /// <param name="addToMap"></param>
        public ExaltedCharm(string modName, bool addToMap) : base(modName, addToMap)
        {
            On.CharmIconList.GetSprite += GetExaltedIcon;
            ModHooks.SavegameSaveHook += Upgrade;
        }

        #region Name
        /// <summary>
        /// Name of the regular charm
        /// </summary>
        public abstract string name { get; }

        /// <summary>
        /// Name of the exalted charm
        /// </summary>
        public abstract string exaltedName { get; }

        /// <summary>
        /// Gets the final name of the charm
        /// </summary>
        /// <returns></returns>
        protected override string GetName()
        {
            if (!IsUpgraded)
            {
                return name;
            }
            else
            {
                return exaltedName;
            }
        }
        #endregion

        #region Description
        /// <summary>
        /// Description of the regular charm
        /// </summary>
        public abstract string description { get; }

        /// <summary>
        /// Description of the exalted charm
        /// </summary>
        public abstract string exaltedDescription { get; }

        /// <summary>
        /// Gets the final description of the charm
        /// </summary>
        /// <returns></returns>
        protected override string GetDescription()
        {
            if (!IsUpgraded)
            {
                return description;
            }
            else
            {
                return exaltedDescription;
            }
        }
        #endregion

        #region Cost
        /// <summary>
        /// Cost of the regular charm
        /// </summary>
        public abstract int cost { get; }

        /// <summary>
        /// Cost of the exalted charm
        /// </summary>
        public abstract int exaltedCost { get; }

        /// <summary>
        /// Gets the final description of the charm
        /// </summary>
        /// <returns></returns>
        protected override int GetCharmCost()
        {
            if (!IsUpgraded)
            {
                return cost;
            }
            else
            {
                return exaltedCost;
            }
        }
        #endregion

        /// <summary>
        /// Gets the string ID of the charm for ItemChanger.
        /// </summary>
        /// <returns></returns>
        public override string GetItemChangerId()
        {
            return Regex.Replace(name, @"[^0-9a-zA-Z\._]", "");
        }

        #region Exaltation
        /// <summary>
        /// Icon of the exalted charm
        /// </summary>
        public abstract Sprite exaltedIcon { get; }

        /// <summary>
        /// Overrides the default GetSprite so that the Exalted Icon can be displayed instead
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Sprite GetExaltedIcon(On.CharmIconList.orig_GetSprite orig, CharmIconList self, int id)
        {
            if (id == Id)
            {
                if (IsUpgraded)
                {
                    return exaltedIcon;
                }
            }

            return orig(self, id);
        }

        /// <summary>
        /// Tracks if the charm has been upgraded
        /// </summary>
        public bool IsUpgraded { get; protected set; }

        /// <summary>
        /// Upon saving, we upgrade the charm if eligible
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void Upgrade(int obj)
        {
            if (IsExaltationInstalled() && 
                CanUpgrade() &&
                GotCharm)
            {
                Upgrade();
            }
        }

        /// <summary>
        /// Checks if Exaltation is installed
        /// </summary>
        /// <returns></returns>
        internal bool IsExaltationInstalled()
        {
            return ModHooks.GetMod("Exaltation") != null;
        }

        /// <summary>
        /// Checks if the charm is eligible for upgrade
        /// </summary>
        /// <returns></returns>
        public abstract bool CanUpgrade();
        
        /// <summary>
        /// Upgrades the charm to its exalted version
        /// </summary>
        public virtual void Upgrade()
        {
            IsUpgraded = true;
        }

        /// <summary>
        /// Replacement for RestoreCharmState that uses ExaltedCharmState
        /// </summary>
        /// <param name="state"></param>
        public void RestoreCharmState(ExaltedCharmState state)
        {
            base.RestoreCharmState(state);
            IsUpgraded = state.IsUpgraded;
        }

        /// <summary>
        /// Replacement for GetCharmState that uses ExaltedCharmState
        /// </summary>
        /// <returns></returns>
        public new ExaltedCharmState GetCharmState()
        {
            return new ExaltedCharmState()
            {
                IsEquipped = IsEquipped,
                IsNew = IsNew,
                IsUpgraded = IsUpgraded,
                GotCharm = GotCharm,
            };
        }
        #endregion
    }
}
