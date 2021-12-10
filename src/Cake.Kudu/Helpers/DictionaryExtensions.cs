using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core.IO;

namespace Cake.Kudu.Helpers
{
    /// <summary>
    /// Extension methods for getting typed values by key from <see cref="IDictionary{TKey, TValue}"/>.
    /// </summary>
    internal static class DictionaryExtensions
    {
        private static readonly ILookup<string, bool?> BoolValues = new[]
        {
            new KeyValuePair<string, bool?>("true", true),
            new KeyValuePair<string, bool?>("1", true),
            new KeyValuePair<string, bool?>("false", false),
            new KeyValuePair<string, bool?>("0", false),
        }.ToLookup(
            key => key.Key,
            value => value.Value,
            StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Try parse <see cref="DirectoryPath"/> from <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="environmenVariables">The environmenVariables.</param>
        /// <param name="key">The key.</param>
        /// <returns>Value as <see cref="DirectoryPath"/> or null.</returns>
        internal static DirectoryPath TryParseDirectoryPath(
            this IDictionary<string, string> environmenVariables,
            string key)
        {
            return (environmenVariables.TryGetValue(key, out string path) &&
                !string.IsNullOrWhiteSpace(path))
                ? new DirectoryPath(path)
                : null;
        }

        /// <summary>
        /// Try parse <see cref="FilePath"/> from <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="environmenVariables">The environmenVariables.</param>
        /// <param name="key">The key.</param>
        /// <returns>Value as <see cref="FilePath"/> or null.</returns>
        internal static FilePath TryParseFilePath(
            this IDictionary<string, string> environmenVariables,
            string key)
        {
            return (environmenVariables.TryGetValue(key, out string path) &&
                !string.IsNullOrWhiteSpace(path))
                ? new FilePath(path)
                : null;
        }

        /// <summary>
        /// Try parse <see cref="bool"/> from <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="environmenVariables">The environmenVariables.</param>
        /// <param name="key">The key.</param>
        /// <returns>Value as <see cref="bool"/> or null.</returns>
        internal static bool? TryParseBool(
            this IDictionary<string, string> environmenVariables,
            string key)
        {
            return (environmenVariables.TryGetValue(key, out string value) &&
                !string.IsNullOrWhiteSpace(value))
                ? BoolValues[value].FirstOrDefault()
                : null;
        }

        /// <summary>
        /// Try parse <see cref="string"/> from <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="environmenVariables">The environmenVariables.</param>
        /// <param name="key">The key.</param>
        /// <returns>Value as <see cref="string"/> or null.</returns>
        internal static string TryGetString(
            this IDictionary<string, string> environmenVariables,
            string key)
        {
            return (environmenVariables.TryGetValue(key, out string value) &&
                !string.IsNullOrWhiteSpace(value))
                ? value
                : null;
        }

        /// <summary>
        /// Try parse <see cref="int"/> from <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="environmenVariables">The environmenVariables.</param>
        /// <param name="key">The key.</param>
        /// <returns>Value as <see cref="int"/> or null.</returns>
        internal static int? TryParseInt(
            this IDictionary<string, string> environmenVariables,
            string key)
        {
            string value;
            int intValue;
            return (environmenVariables.TryGetValue(key, out value) &&
                    !string.IsNullOrWhiteSpace(value))
                ? (int.TryParse(value, out intValue)
                    ? intValue as int?
                    : null)
                : null;
        }
    }
}
