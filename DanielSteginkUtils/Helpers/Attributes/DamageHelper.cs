using UnityEngine;

namespace DanielSteginkUtils.Helpers.Attributes
{
    /// <summary>
    /// Code library for sending damaging attacks to an enemy
    /// </summary>
    public static class DamageHelper
    {
        /// <summary>
        /// Damages the enemy with an attack using the given properties
        /// </summary>
        /// <param name="enemy"></param>
        /// <param name="amount"></param>
        /// <param name="attackType"></param>
        /// <param name="attackName"></param>
        public static void DealDamage(HealthManager enemy, int amount, AttackTypes attackType, string attackName)
        {
            // Create a temporary game object with the attack's name
            GameObject attacker = new GameObject(attackName);

            // Apply the damage
            DealDamage(enemy, amount, attackType, attacker);

            // Destroy the object
            Object.Destroy(attacker);
        }

        /// <summary>
        /// Damages the enemy with an attack using the given properties
        /// </summary>
        /// <param name="enemy"></param>
        /// <param name="amount"></param>
        /// <param name="attackType"></param>
        /// <param name="attacker"></param>
        public static void DealDamage(HealthManager enemy, int amount, AttackTypes attackType, GameObject attacker)
        {
            HitInstance hit = new HitInstance
            {
                DamageDealt = amount,
                AttackType = attackType,
                IgnoreInvulnerable = true,
                Source = attacker,
                Multiplier = 1f
            };

            enemy.Hit(hit);
        }
    }
}