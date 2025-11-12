# DanielSteginkUtils

A code library containing various helper classes, balance calculations, and logic I've accumulated in my modding journey.

General structure:
- Components - Custom components that combine with Helpers to modify certain properties
	- Dung
		- BuffDungDamage - Modifies damage rate of Defender's Crest
		- BuffDungFluke - Modifies damage rate of Defender's Crest + Flukenest
		- BuffDungSize - Modifies size of Defender's Crest
	- Shroom
		- BuffSporeDamage - Modifies damage rate of Spore SHroom
		- BuffSporeSize - Modifies size of Spore Shroom
	- BuffAspidAspect - Modifies damage of Aspid Aspect
	- ExtraJumps - Modifies number of jumps received from Monarch Wings
	- ModBuffs - Template for custom components
- Helpers - Helper objects for modifying various properties
	- Abilities
		- DashHelper - Modifies cooldowns of Mothwing Cloak and Shade Cloak
		- JumpHelper - Modifies number of jumps received from Monarch Wings
	- Attributes
		- DamageHelper - Creates a HitInstance to deal damage to an enemy
		- GeoHelper - Modifies Geo gained from all sources
		- HealHelper - Adds a chance to get an additional Mask when healing
		- HealingSpeedHelper - Modifies the time required to heal
		- NailArtRangeHelper - Modifies the range of Nail Arts
		- NailRangeHelper - Modifies the range of nail attacks
		- SoulHelper - Gives extra SOUL to the player without triggering related events
		- SpeedHelper - Modifies movement speed
		- StaggerHelper - Modifies the number of hits required to stagger a boss
	- Charms
		- Dung
			- DungDamageHelper - Modifies damage rate of Defender's Crest
			- DungFlukeHelper - Modifies damage rate of Defender's Crest + Flukenest
			- DungSizeHelper - Modifies size of Defender's Crest
		- Elegy
			- ElegyBeamAttacker - Adds a chance to trigger a Grubberfly's Elegy beam attack when swinging the nail
			- ElegyBeamRangeHelper - Modifies the range of Grubberfly's Elegy beam attacks
		- Pets
			- AllPetsHelper - Modifies damage rate of Flukenest, Grimmchild, Glowing Womb, Weaversong, Aspid Aspect and Flukenest + Defender's Crest
			- AspidAspectHelper - Modifies damage of Aspid Aspect
			- Flukehelper - Modifies damage of Flukenest
			- GrimmchildHelper - Modifies damage and attack speed of Grimmchild
			- HatchlingHelper - Modifies damage of Glowing Womb
			- WeaverlingHelper - Modifies damage of Weaversong
		- Shroom
			- SporeDamageHelper - Modifies damage rate of Spore SHroom
			- SporeSizeHelper - Modifies size of Spore Shroom
		- Templates
			- TemplateCharm - Variation of SFCore's EasyCharm. Includes logic for adding the charm to the map for pickup
			- ExaltedCharm - Variation of TemplateCharm that also includes logic for upgrading the charm via the Exaltation mod
			- ExaltedCharmState - Variation of SFCore's EasyCharmState that also handles if a charm has been upgraded via Exaltation
		- GetModCharmHelper - Gets the numeric ID of a Mod-added charm by name
	- Shields
		- ShieldHelper - Template for adding new damage-blocking abilities
		- CarefreeHelper - Adds a second chance of Carefree Melody triggering
	- CustomBuffHelper - Template for dynamically modifying a property
	- GetEnemyHelper - Gets the nearest enemy
	- SpriteHelper - Gets a sprite from embedded resources
- Utilities - Various libraries for logic and calculations
	- Calculations - Performs calculations such as converting damage values to other types
	- ClassIntegrations - Accesses properties, fields and methods from other classes
	- Logic - Determines logic results such as if an object is a nail attack or if the player can take damage
	- NotchCosts - Calculates the value of various properties and modifiers in terms of charm notches
	- PlayerValues - Gets properties related to the player, such as how much SOUL the player has collected

## Thanks
Thank you to SFGrenade, Roma 337, Volt, hannes-j, YuliaS11 and Spamtom F. Gambleton for testing and feedback.

## Patch Notes
1.4.2.1
- Bug fix for StaggerHelper to prevent stacking

1.4.2.0
- Bug fix for FSM code to avoid the "loop count exceeded maximum" error

1.4.1.0
- Bug fix for NailArtRangeHelper

1.4.0.0
- Added AspidAspectHelper and integrated it into AllPetsHelper
- Bug fix for PassiveSoulTime

1.3.1.0
- Bug fix for AllPetsHelper

1.3.0.0
- Modified DashHelper to use HKMirror's HeroControllerR
- Fixed Readme
- Reworked NailArtRangeHelper and ElegyBeamRangeHelper

1.2.2.0
- Fix for DashHelper so it doesn't get overwritten by CharmChanger
- Redid TemplateCharm to use ItemChanger

1.2.1.2
- Icon bug fix v3
- Various small changes to helpers

1.2.1.1
- Icon bug fix v2

1.2.1.0
- Icon bug fix for ExaltedCharm

1.2.0.0
- Added/sorted helpers for Grubberfly's Elegy
- New logic for checking if an attack is a Nail Art
- Bug fix for IsNailAttack
- Modified load priority so this library loads before most mods

1.1.0.0
- Added Charm templates using SFCore's EasyCharm as a base
- Added a new modifier for Shape of Unn to NotchCosts

1.0.1.0
- Replaced references to PlayerData with calls to GetBool and GetInt
- Simplified code in GetModCharmHelper
