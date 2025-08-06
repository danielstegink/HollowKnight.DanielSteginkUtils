using DanielSteginkUtils.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DanielSteginkUtils.Helpers
{
    /// <summary>
    /// Helper for getting sprites
    /// </summary>
    public static class SpriteHelper
    {
        /// <summary>
        /// Gets sprite from embedded resources
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="assemblyName"></param>
        /// <param name="performLogging"></param>
        /// <returns></returns>
        public static Sprite GetLocalSprite(string filePath, string assemblyName = "", bool performLogging = false)
        {
            //SharedData.Log($"Getting local sprite {spriteId}");
            if (performLogging)
            {
                Logging.Log("SpriteHelper", $"Getting sprite {filePath} in assembly '{assemblyName}'");
            }

            Assembly assembly = GetAssembly(assemblyName);
            if (assembly == null)
            {
                Logging.Log("SpriteHelper", $"Assembly '{assemblyName}' not found");
                return null;
            }

            using (Stream stream = assembly.GetManifestResourceStream(filePath))
            {
                // Convert stream to bytes
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);

                // Create texture from bytes
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(bytes, true);

                // Create sprite from texture
                return Sprite.Create(texture,
                                        new Rect(0, 0, texture.width, texture.height),
                                        new Vector2(0.5f, 0.5f));
            }
        }

        /// <summary>
        /// Gets assembly holding embedded resources
        /// </summary>
        /// <param name="assemblyName">If blank, will get executing assembly instead</param>
        /// <returns></returns>
        private static Assembly GetAssembly(string assemblyName = "")
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                return Assembly.GetExecutingAssembly();
            }
            else
            {
                return Assembly.Load(assemblyName);
            }
        }
    }
}
