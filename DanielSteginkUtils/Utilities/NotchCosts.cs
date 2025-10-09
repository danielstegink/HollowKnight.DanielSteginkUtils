namespace DanielSteginkUtils.Utilities
{
    /// <summary>
    /// Library for calculating the value of different abilities in terms of Charm notches
    /// </summary>
    public static class NotchCosts
    {
        #region Health
        /// <summary>
        /// The percent increase in Healing speed per notch
        /// </summary>
        /// <returns></returns>
        public static float HealingSpeedPerNotch()
        {
            // Quick Focus increases healing speed by 33% for 3 notches
            // So 1 notch is worth 11%
            float quickFocusModifier = 0.594f / 0.891f;

            return (1 - quickFocusModifier) / 3f;
        }

        /// <summary>
        /// The number of notches it is worth to heal 1 Mask
        /// </summary>
        /// <returns></returns>
        public static float NotchesPerHeal()
        {
            // Deep Focus is worth 4 notches and doubles healing, which means we heal 1 extra Mask
            float notchCount = 4;

            // Additionally, our healing time is increased by 165%. Reducing this back to 100% would require a 40% reduction in healing speed.
            float healTime = 1.65f;
            float healSpeedModifier = 1 - 1 / healTime;

            // Quick Focus reuces healing time by 33% for 3 nothces, so a 40% reduction would be worth about 4 notches.
            notchCount += healSpeedModifier / HealingSpeedPerNotch();

            // In total, the ability to heal an extra mask is worth 8 notches.
            return notchCount;
        }

        /// <summary>
        /// The number of notches a Mask is worth
        /// </summary>
        /// <returns></returns>
        public static float NotchesPerMask()
        {
            // Unbreakable Heart is worth 4 notches per my Unbreakable logic, so 1 mask is worth 2 notches
            return UnbreakableCharmCost(2) / 2f;
        }

        /// <summary>
        /// The number of notches a Lifeblood Mask is worth
        /// </summary>
        /// <returns></returns>
        public static float NotchesPerBlueMask()
        {
            // Lifeblood Core is the most endgame of the Lifeblood charms and gives 4 masks for 3 notches
            // So 1 Lifeblood mask is worth 0.75 notches
            return 3f / 4f;
        }
        #endregion

        #region Damage
        /// <summary>
        /// The percent increase in nail damage the player should get per notch
        /// </summary>
        /// <returns></returns>
        public static float NailDamagePerNotch()
        {
            // Unbreakable Strength increases nail damage by 50%
            // It costs 3 notches, but per UnbreakableCharmCost its value should be 5 notches
            // That means a 10% increase in nail damage per notch.
            return 0.5f / UnbreakableCharmCost(3);
        }

        /// <summary>
        /// The percent increase in nail range the player should get per notch
        /// </summary>
        /// <returns></returns>
        public static float NailRangePerNotch()
        {
            // MOP gives a 25% increase for 3 notches.
            // So 1 notch is worth a 8.33% increase
            return 0.25f / 3f;
        }

        /// <summary>
        /// The percent increase in Nail Art damage the player should get per notch
        /// </summary>
        /// <returns></returns>
        public static float NailArtDamagePerNotch()
        {
            // For 1 notch, NMG reduces NA charge time by 44%
            float nmgPerNotch = 0.75f / 1.35f;

            // For 3 notches, Quick Slash reduces nail cooldown by 39%
            float qsPerNotch = 0.25f / 0.41f / 3f;

            // Comparing these 2, an increase in NA damage should be 2.73 times that of an increase in regular nail damage.
            float nailArtToNailRatio = nmgPerNotch / qsPerNotch;

            // So if 1 notch gives a 10% nail damage boost, it should give about a 27% boost in Nail Art damage
            return NailDamagePerNotch() * nailArtToNailRatio;
        }

        /// <summary>
        /// The percent damage increase that all spells should get per notch
        /// </summary>
        /// <returns></returns>
        public static float SpellDamagePerNotch()
        {
            // Shaman Stone increases each spell's damage by a different percent:
            // Vengeful Spirit gets 33%
            // Desolate Dive gets 51%
            // Howling Wraiths gets 50%

            // However, it also increases the size of Vengeful Spirit projectiles
            // If it didn't do this, it is somewhat reasonable to assume that
            // Vengeful Spirit's boost would also be about 50%

            // So for 1 notch, we should increase all spell damage by 50 / 3 = 16.67%
            return 0.5f / 3f;
        }

        /// <summary>
        /// The percent damage increase a single spell should get per notch
        /// </summary>
        /// <returns></returns>
        public static float SingleSpellDamagePerNotch()
        {
            // Per above, Shaman Stone increases all spells by an average of 50% for 3 notches
            // It applies this increase to 3 groups of spells, so applying it to just one of them would be worth 1 notch
            // So for 1 notch we can increase the damage of a single spell by 50%
            return 0.5f;
        }

        /// <summary>
        /// The percent increase in all damage the player should get per notch
        /// </summary>
        /// <returns></returns>
        public static float DamagePerNotch()
        {
            // Per above, an increase in Spell Damage is worth 16.67% per notch
            // while an increase in Nail Damage is worth 10% per notch

            // Applying the average of both of these would be 13.33% per notch
            // However, it would cost 2 notches to apply this effect, so the 
            // actual per-notch value is 6.67%

            return (NailDamagePerNotch() + SpellDamagePerNotch()) / 4;
        }

        /// <summary>
        /// The percent increase in pet damage the player should get per notch
        /// </summary>
        /// <returns></returns>
        public static float PetDamagePerNotch()
        {
            // There are 4 pet charms in the base game: Grimmchild, Glowing Womb, Weaversong and Flukenest
            // Together, they represent 9 notches worth of charms
            // Let's assume 1 of them isn't equipped to encourage build diversity, bringing the total to about 6 notches
            // A 100% increase in pet damage would be like having each one equipped twice, doubling the number of pets
            // So a 100% increase in pet damage is worth 6 notches, making 1 notch worth 16.67%
            return 1f / 6f;
        }
        #endregion

        #region SOUL
        /// <summary>
        /// The flat increase in SOUL per nail swing per notch
        /// </summary>
        /// <returns></returns>
        public static float SoulPerNailPerNotch()
        {
            // Soul Eater increases SOUL gained from nail swings by 8 for 4 notches.
            // That means 1 notch is worth 2 SOUL per nail hit
            return 8f / 4f;
        }

        /// <summary>
        /// The percent increase in SOUL gained from Dream Nail per notch
        /// </summary>
        /// <returns></returns>
        public static float DreamNailSoulPerNotch()
        {
            // Dream Wielder is worth 1 notch and has 3 effects:
            // 1) Reduces Dream Nail charge time
            // 2) Increases SOUL gained from Dream Nail by 100%
            // 3) Increases the chance of an enemy dropping Essence upon death
            // My personal opinion on the value ratio of these 3 is 50%, 33.33%, and 16.67%, in that order.

            // So 1 notch is worth a 300% increase in SOUL gained
            return 3f; 
        }

        /// <summary>
        /// The length of time it should take to generate 1 point of SOUL for 1 notch
        /// </summary>
        /// <returns></returns>
        public static float PassiveSoulTime()
        {
            // For 5 notches, Kingsoul gives 4 SOUL every 2 seconds, or 1 SOUL per 0.5 seconds
            float kingsoulTime = 2f / 4f;

            // So for 1 notch, the player should get 0.4 SOUL per second, or 1 SOUL every 2.5 seconds
            return kingsoulTime * 5;
        }
        #endregion

        #region Dash
        /// <summary>
        /// The flat increase in dash damage a player should get per notch
        /// </summary>
        /// <returns></returns>
        public static float DashDamagePerNotch()
        {
            // Sharp Shadow and CDash are both dash-like attacks that deal damage.
            // CDash is an ability, and therefore doesn't have a notch comparison.

            // But Sharp Shadow buffs DDash to deal nail damage for 2 notches, 
            // so logically 1 notch is worth 1/2 nail damage.
            float ssBonus = PlayerData.instance.GetInt("nailDamage");
            return ssBonus / 2;
        }

        /// <summary>
        /// The percent decrease in dash cooldown that a player should get per notch
        /// </summary>
        /// <returns></returns>
        public static float GetDashCooldownPerNotch()
        {
            // Dashmaster reduces dash cooldown by 33.33% for 2 notches, so 1 notch is worth 16.67%
            float dashmasterModifier = 1 - 0.4f / 0.6f;

            return dashmasterModifier / 2;
        }
        #endregion

        /// <summary>
        /// The number of notches an Unbreakable charm would be worth without the cost of making it unbreakable
        /// </summary>
        /// <param name="baseCost"></param>
        /// <returns></returns>
        public static int UnbreakableCharmCost(int baseCost)
        {
            // I haven't found logic for this that satisfies me
            // Going with my gut, I think each fragile charm would be worth 2 extra notches if it didn't break when you died
            return baseCost + 2;
        }

        /// <summary>
        /// The number of notches to reduce a boss's stagger requirements by 1
        /// </summary>
        /// <returns></returns>
        public static int NotchesPerStagger()
        {
            // Heavy Blow costs 2 notches and has 2 abilities: reduce stagger cost, and increase knockback
            // If we assume both abilities are equally valuable, then reducing the stagger cost is worth 1 notch per point.
            return 1;
        }

        /// <summary>
        /// The percent of extra geo the player should get per notch
        /// </summary>
        /// <returns></returns>
        public static float GeoPerNotch()
        {
            // Unbreakable Greed gives 20% more geo from enemies.
            float greedModifier = 0.2f;

            // Per my logic above, it costs 4 notches instead of 2. That's 5% per notch.
            return greedModifier / UnbreakableCharmCost(2);
        }

        /// <summary>
        /// Multipler for the notch value of an ability that requires the player be at full health
        /// </summary>
        /// <returns></returns>
        public static float FullHealthModifier()
        {
            // For 3 notches, Grubberfly's Elegy shoots a beam that travels 300% the normal range of a nail attack and deals 50% damage

            // Per the logic in NailLengthPerNotch, a 200% range increase is worth about 24 notches
            float rangeCost = 2 / NailRangePerNotch();

            // However, this only applies to the beam, which deals only 1/3 of the total damage dealt
            // So in reality, the range increase is worth 8 notches. 
            rangeCost /= 3;

            // If we factor in the chance of missing due to the time delay of the attack, it becomes about 7 notches.
            rangeCost--;

            // Per NailDamagePerNotch, a 50% increase in nail damage is worth 5 notches
            float damageCost = 0.5f / NailDamagePerNotch();

            // So the total value of Grubberfly's Elegy, if it didn't require full health, would be 12 notches
            float totalCost = rangeCost + damageCost;

            // Since GE is worth 3 notches, requiring full health reduces the value of an ability by a factor of 4
            return totalCost / 3f;
        }

        /// <summary>
        /// Percent chance of blocking an attack for the given number of notches, as a number between 0 and 100
        /// </summary>
        /// <returns></returns>
        public static float ShieldChancePerNotch()
        {
            // Carefree Melody gives an average chance of 22.46% for 3 notches
            // That's 7.49% per notch
            return 22.46f / 3;
        }

        /// <summary>
        /// Percent increase in I-Frames player should get per notch
        /// </summary>
        /// <returns></returns>
        public static float IFramesPerNotch()
        {
            // Stalwart Shell is a 2-notch charm with 2 effects, one of which is increasing I-Frames from
            // 1.3 seconds to 1.75, a 35% increase.
            float stalwartShellModifier = 1.75f / 1.3f - 1;

            // If we assume the effects are equal, that means 1 notch is worth a 35% boost in I-Frames.
            return stalwartShellModifier;
        }

        /// <summary>
        /// Flat multiplier in the cost of a property that only triggers during Shape of Unn
        /// </summary>
        /// <returns></returns>
        public static float UnnModifier()
        {
            // For 1 notch, Sprintmaster increases movement speed by 20%
            // However, it increases SOU's speed by 100%
            // That means a property or modifier is 5x as valuable when it is only active during SOU,
            // or 20% of its cost
            return 0.2f;
        }
    }
}