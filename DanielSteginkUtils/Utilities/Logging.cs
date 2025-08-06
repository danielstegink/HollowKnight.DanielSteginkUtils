using System;

namespace DanielSteginkUtils.Utilities
{
    /// <summary>
    /// Stores logging utilities for the rest of the library
    /// </summary>
    internal static class Logging
    {
        /// <summary>
        /// Custom logging for the helper
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="message"></param>
        internal static void Log(string prefix, string message)
        {
            DanielSteginkUtils.Instance.Log($"{prefix} - {message}");
        }
    }
}
