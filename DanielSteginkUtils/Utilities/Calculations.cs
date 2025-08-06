using System;

namespace DanielSteginkUtils.Utilities
{
    /// <summary>
    /// Library for mechanical calculations I've used frequently
    /// </summary>
    public static class Calculations
    {
        /// <summary>
        /// Gets the modded value as an integer (minimum change of 1)
        /// </summary>
        /// <param name="baseValue"></param>
        /// <param name="modifier"></param>
        /// <returns></returns>
        public static int GetModdedInt(int baseValue, float modifier)
        {
            float moddedValue = baseValue * modifier;
            if (modifier > 1)
            {
                return (int)Math.Max(baseValue + 1, moddedValue);
            }
            else if (modifier == 1)
            {
                return baseValue;
            }
            else
            {
                return (int)Math.Min(baseValue - 1, moddedValue);
            }
        }

        /// <summary>
        /// Takes the amount to increase Carefree Melody's probability by
        /// and converts it to the balanced probability of a second shield occurring
        /// </summary>
        /// <param name="bonus"></param>
        /// <returns></returns>
        public static int GetSecondMelodyShield(float bonus)
        {
            // CM has a 22.46% chance of occurring by default
            // (1 - CM) * (1 - X) = 1 - (CM + bonus), which is the chance of both failing
            // 0.77 * (1 - X) = 0.77 - bonus
            // 1 - X = (0.77 - bonus) / 0.77
            // X = 1 - (0.77 - bonus) / 0.77
            float cmDecimal = 22.46f / 100f;
            float bonusDecimal = bonus / 100f;
            float secondCmDecimal = 1 - (1 - cmDecimal - bonusDecimal) / (1f - cmDecimal);
            return (int)(secondCmDecimal * 100f);
        }

        /// <summary>
        /// Converts nail damage to an equivalent amount as spell damage
        /// </summary>
        /// <param name="nailDamage"></param>
        /// <returns></returns>
        public static float NailDamageToSpellDamage(float nailDamage)
        {
            // Nail is a fast, close-range attack lasting about 0.41 seconds
            // Fully upgraded, it deals 21 damage

            // The most comparable spell is Abyss Shriek
            // Per the Spell Control FSM, Shriek lasts about 0.45 seconds from start to finish
            // Shriek creates 4 hit boxes that deal 20 damage each, which each able to hit an enemy for a total of 80 damage
            // However, the boxes can miss even at close range, so the average damage is probably closer to 60

            // So the Nail deals 21 damage every 0.41 seconds while Shriek deals 60 damage every 0.45 seconds
            // That means the DPS for the nail is 48.78 while Shriek's is 133.33
            // That makes the ratio of nail damage to spell damage about 2.73
            float nailDps = 21f / 0.41f;
            float spellDps = 60f / 0.45f;
            float ratio = spellDps / nailDps;

            return nailDamage * ratio;
        }

        /// <summary>
        /// Converts nail damage to a proportional amount if attacking with the Dream Nail
        /// </summary>
        /// <param name="nailDamage"></param>
        /// <returns></returns>
        public static float NailDamageToDreamNailDamage(float nailDamage)
        {
            // Dream Nail does not damage of course, but it takes much longer to attack,
            // so if the Dream Nail were to deal damage, it would deal more than a basic nail attack

            // A regular nail takes 0.41 seconds to deal damage, whereas Dream Nail takes 1.75 seconds
            // So if the Dream Nail were to deal damage, it would deal 4.26 times as much damage
            float ratio = 1.75f / 0.41f;

            return nailDamage * ratio;
        }

        /// <summary>
        /// Enum for the different spells
        /// </summary>
        public enum SpellType
        {
            /// <summary>
            /// Vengeful Spirit
            /// </summary>
            VengefulSpirit,

            /// <summary>
            /// Desolate Dive
            /// </summary>
            DesolateDive,

            /// <summary>
            /// Howling Wraiths
            /// </summary>
            HowlingWraiths,

            /// <summary>
            /// Shade Soul
            /// </summary>
            ShadeSoul,

            /// <summary>
            /// Descending Dark
            /// </summary>
            DescendingDark,

            /// <summary>
            /// Abyss Shriek
            /// </summary>
            AbyssShriek
        }

        /// <summary>
        /// Damage per SOUL spent for each spell
        /// </summary>
        /// <returns></returns>
        public static float DamagePerSoul(SpellType spellType)
        {
            switch (spellType)
            {
                case SpellType.VengefulSpirit:
                    return 15f / 33f;
                case SpellType.DesolateDive:
                    return 35f / 33f;
                case SpellType.HowlingWraiths: // Shriek is known to miss on occasion, so its average damage is less than the max
                    return 40f / 33f;
                case SpellType.ShadeSoul:
                    return 30f / 33f;
                case SpellType.DescendingDark:
                    return 60f / 33f;
                case SpellType.AbyssShriek:
                    return 60f / 33f;
                default:
                    throw new ArgumentException("Spell Type not recognized.");
            }
        }

        /// <summary>
        /// The amount of SOUL one could save in exchange for using 1 Essence
        /// </summary>
        /// <returns></returns>
        public static float SoulPerEssence()
        {
            // Normally the user gets 11 SOUL per nail attack
            // Assuming the enemy can survive 3 nail attacks (about 60 HP w/ Pure Nail), thats 33 SOUL per enemy

            // In comparison, Essence has a drop rate of 1 per 60 enemies, assuming a lot of Essence has been used
            // On paper, this makes it very valuable
            // However, we use SOUL constantly and use Essence practically never, so the low demand reduces its value
            // Going off vibes due to a lack of comparison, I've decided to reduce the value of Essence by 99%
            // 33 * 60 / 100 = 19.8

            return 33f * 60f / 100f;
        }
    }
}