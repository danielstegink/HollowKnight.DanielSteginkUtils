using GlobalEnums;
using System;
using System.Linq;
using UnityEngine;

namespace DanielSteginkUtils.Helpers
{
    /// <summary>
    /// Library for getting enemies
    /// </summary>
    public static class GetEnemyHelper
    {
        /// <summary>
        /// Gets the closest enemy that can take damage
        /// </summary>
        /// <returns></returns>
        public static GameObject GetNearestEnemy()
        {
            // Get all active game objects
            GameObject[] gameObjects = UnityEngine.Component.FindObjectsOfType<GameObject>()
                                                                .Where(x => x.activeSelf).ToArray();
            if (gameObjects.Length == 0)
            {
                return null;
            }

            // Filter out game objects that aren't enemies
            GameObject[] enemies = gameObjects.Where(x => IsEnemy(x)).ToArray();
            if (enemies.Length == 0)
            {
                return null;
            }

            // Determines which enemy is closest to the player
            Transform playerPosition = HeroController.instance.gameObject.transform;
            GameObject closestEnemy = enemies.OrderBy(x => GetDistance(x.transform, playerPosition))
                                                .FirstOrDefault();

            return closestEnemy;
        }

        /// <summary>
        /// Determines if an object is an enemy
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static bool IsEnemy(GameObject gameObject)
        {
            bool isEnemy = false;
            if (gameObject.layer == (int)PhysLayers.ENEMIES ||
                    //gameObject.layer == 17 || todo - which enemies are in the hero layer?
                    gameObject.tag == "Boss")
            {
                // Enemy must have a health bar for us to consider them a damageable enemy
                HealthManager health = gameObject.GetComponent<HealthManager>();
                if (health != default)
                {
                    isEnemy = true;
                }
            }

            return isEnemy;
        }

        /// <summary>
        /// Gets the distance between 2 objects
        /// </summary>
        /// <param name="enemy"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public static float GetDistance(Transform enemy, Transform player)
        {
            float xDiff = Math.Abs(enemy.GetPositionX() - player.GetPositionX());
            float yDiff = Math.Abs(enemy.GetPositionY() - player.GetPositionY());
            return (float)Math.Sqrt(xDiff * xDiff + yDiff * yDiff);
        }
    }
}
