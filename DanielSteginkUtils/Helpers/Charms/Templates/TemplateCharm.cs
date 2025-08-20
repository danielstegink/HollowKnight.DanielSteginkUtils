using DanielSteginkUtils.Utilities;
using ItemChanger;
using ItemChanger.Tags;
using ItemChanger.UIDefs;
using Modding;
using SFCore;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DanielSteginkUtils.Helpers.Charms.Templates
{
    /// <summary>
    /// Template class for upgradeable custom charms based on SFCore's EasyCharm
    /// </summary>
    public abstract class TemplateCharm : EasyCharm
    {
        /// <summary>
        /// Template class for upgradeable custom charms based on SFCore's EasyCharm
        /// </summary>
        /// <param name="modName"></param>
        /// <param name="addToMap"></param>
        public TemplateCharm(string modName, bool addToMap) : base()
        {
            Logging.Log(modName, $"Charm added under the numeric ID {Id}");

            ModHooks.SetPlayerBoolHook += CheckEquipped;
            On.HeroController.Start += OnPlayerDataLoaded;

            AddToItemChanger(modName, addToMap);
        }

        #region ItemChanger
        /// <summary>
        /// Adds the charm to ItemChanger
        /// </summary>
        /// <param name="modName"></param>
        /// <param name="addToMap"></param>
        private void AddToItemChanger(string modName, bool addToMap)
        {
            // Add the charm and its location to ItemChanger for placement
            ItemChanger.Items.CharmItem item = new ItemChanger.Items.CharmItem()
            {
                charmNum = Id,
                name = GetItemChangerId(),
                UIDef = new MsgUIDef()
                {
                    name = new LanguageString("UI", $"CHARM_NAME_{Id}"),
                    shopDesc = new LanguageString("UI", $"CHARM_DESC_{Id}"),
                    sprite = new BoxedSprite(GetSprite())
                }
            };

            var mapModTag = item.AddTag<InteropTag>();
            mapModTag.Message = "RandoSupplementalMetadata";
            mapModTag.Properties["ModSource"] = modName;
            mapModTag.Properties["PoolGroup"] = "Charms";

            Finder.DefineCustomItem(item);

            if (addToMap)
            {
                // Add the charm's custom location to Finder
                Finder.DefineCustomLocation(ItemChangerLocation());

                // Add the charm
                On.GameManager.EnterHero += AddToMap;
            }
        }

        /// <summary>
        /// Gets the string ID of the charm for ItemChanger.
        /// </summary>
        /// <returns></returns>
        public virtual string GetItemChangerId()
        {
            return Regex.Replace(GetName(), @"[^0-9a-zA-Z\._]", "");
        }

        /// <summary>
        /// The abstract location used by ItemChanger to add the charm to the map
        /// </summary>
        /// <returns></returns>
        public abstract AbstractLocation ItemChangerLocation();

        /// <summary>
        /// Adds charm to map so it can be picked up
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        /// <param name="additiveGateSearch"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void AddToMap(On.GameManager.orig_EnterHero orig, GameManager self, bool additiveGateSearch)
        {
            ItemChangerMod.CreateSettingsProfile(false, false);
            AbstractLocation location = Finder.GetLocation(GetItemChangerId());
            AbstractPlacement placement = location.Wrap();
            placement.Add(Finder.GetItem(GetItemChangerId()));

            List<AbstractPlacement> placements = new List<AbstractPlacement>()
            {
                placement
            };
            ItemChangerMod.AddPlacements(placements, PlacementConflictResolution.Ignore);

            orig(self, additiveGateSearch);
        }
        #endregion

        #region Activation
        /// <summary>
        /// Tracks when the charm is equipped or unequipped
        /// </summary>
        /// <param name="name"></param>
        /// <param name="orig"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CheckEquipped(string name, bool orig)
        {
            if (name.Equals($"equippedCharm_{Id}"))
            {
                if (orig)
                {
                    Equip();
                }
                else
                {
                    Unequip();
                }
            }

            return orig;
        }

        /// <summary>
        /// When the player data has been loaded and the HeroController has started, we can equip the charm
        /// if it was equipped when we last closed the game
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        private void OnPlayerDataLoaded(On.HeroController.orig_Start orig, HeroController self)
        {
            if (IsEquipped)
            {
                Equip();
            }

            orig(self);
        }

        /// <summary>
        /// Activates the charm effects
        /// </summary>
        public abstract void Equip();

        /// <summary>
        /// Deactivates the charm effects
        /// </summary>
        public abstract void Unequip();
        #endregion

        #region Save Settings
        /// <summary>
        /// Loads settings (see EasyCharmState or ExaltedCharmState) from local save data
        /// </summary>
        public abstract void OnLoadLocal();

        /// <summary>
        /// Saves settings (see EasyCharmState or ExaltedCharmState) to local save data
        /// </summary>
        public abstract void OnSaveLocal();
        #endregion
    }
}
