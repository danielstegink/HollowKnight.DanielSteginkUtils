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
    /// Template class for upgradeable custom charms based on SFCore's EasyCharm.
    /// 
    /// WARNING - Uses ItemChanger to add charms to the map, so making a new save will be required.
    /// </summary>
    public abstract class TemplateCharm : EasyCharm
    {
        /// <summary>
        /// Stores the name of the charm's mod for logging purposes
        /// </summary>
        private string ModName { get; set; }

        /// <summary>
        /// Template class for custom charms based on SFCore's EasyCharm
        /// </summary>
        /// <param name="modName"></param>
        /// <param name="addToMap"></param>
        public TemplateCharm(string modName, bool addToMap) : base()
        {
            ModName = modName;
            Logging.Log(ModName, $"Charm added under the numeric ID {Id}");

            ModHooks.SetPlayerBoolHook += CheckEquipped;
            On.HeroController.Start += OnPlayerDataLoaded;

            if (addToMap)
            {
                AddCharmToItemChanger();
                On.UIManager.StartNewGame += NewGame;
            }
        }

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
        /// if it was equipped when we last closed the game.
        /// 
        /// This is also the best time to add the charm to the map, since we can check the save data to see if the
        /// charm has been acquired already
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        private void OnPlayerDataLoaded(On.HeroController.orig_Start orig, HeroController self)
        {
            orig(self);

            if (IsEquipped)
            {
                Equip();
            }
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

        #region ItemChanger
        /// <summary>
        /// Gets the string ID of the charm for ItemChanger.
        /// </summary>
        /// <returns></returns>
        public virtual string GetItemChangerId()
        {
            return Regex.Replace(GetName(), @"[^0-9a-zA-Z\._]", "");
        }

        /// <summary>
        /// Adds the charm to ItemChanger for easy reference
        /// </summary>
        internal void AddCharmToItemChanger()
        {
            var charmItem = new ItemChanger.Items.CharmItem()
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

            var mapModTag = charmItem.AddTag<InteropTag>();
            mapModTag.Message = "RandoSupplementalMetadata";
            mapModTag.Properties["ModSource"] = ModName;
            mapModTag.Properties["PoolGroup"] = "Charms";

            Finder.DefineCustomItem(charmItem);
            Finder.DefineCustomLocation(ItemChangerLocation());
            //Logging.Log(ModName, "Charm added to ItemChanger");
        }

        /// <summary>
        /// The abstract location used by ItemChanger to add the charm to the map
        /// </summary>
        /// <returns></returns>
        public abstract AbstractLocation ItemChangerLocation();

        private void NewGame(On.UIManager.orig_StartNewGame orig, UIManager self, bool permaDeath, bool bossRush)
        {
            PlaceCharms();
            orig(self, permaDeath, bossRush);
        }

        /// <summary>
        /// Gets charms from ItemChanger and places them on the map
        /// </summary>
        public void PlaceCharms()
        {
            if (!GotCharm)
            {
                ItemChangerMod.CreateSettingsProfile(false, false);
                List<AbstractPlacement> placements = new List<AbstractPlacement>();

                AbstractLocation location = Finder.GetLocation(GetItemChangerId());
                AbstractPlacement placement = location.Wrap();
                AbstractItem item = Finder.GetItem(GetItemChangerId());
                placement.Add(item);

                placements.Add(placement);
                ItemChangerMod.AddPlacements(placements, PlacementConflictResolution.Replace);
            }
        }
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