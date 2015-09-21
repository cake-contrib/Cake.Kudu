using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core.IO;

namespace Cake.Kudu.Helpers
{
    internal static class DictionaryExtensions
    {
        internal static DirectoryPath TryParseDirectoryPath(
            this IDictionary<string, string> environmenVariables,
            string key
            )
        {
            string path;
            return (environmenVariables.TryGetValue(key, out path) &&
                !string.IsNullOrWhiteSpace(path))
                ? new DirectoryPath(path)
                : null;
        }
        internal static FilePath TryParseFilePath(
            this IDictionary<string, string> environmenVariables,
            string key
            )
        {
            string path;
            return (environmenVariables.TryGetValue(key, out path) &&
                !string.IsNullOrWhiteSpace(path))
                ? new FilePath(path)
                : null;
        }

        private static readonly ILookup<string, bool?> BoolValues = new[]
        {
            new KeyValuePair<string, bool?>("true", true),
            new KeyValuePair<string, bool?>("1", true),
            new KeyValuePair<string, bool?>("false", false),
            new KeyValuePair<string, bool?>("0", false)
        }.ToLookup(
            key => key.Key,
            value => value.Value,
            StringComparer.OrdinalIgnoreCase
            );

        internal static bool? TryParseBool(
            this IDictionary<string, string> environmenVariables,
            string key
            )
        {
            string value;
            return (environmenVariables.TryGetValue(key, out value) &&
                !string.IsNullOrWhiteSpace(value))
                ? BoolValues[value].FirstOrDefault()
                : null;
        }

        internal static string TryGetString(
            this IDictionary<string, string> environmenVariables,
            string key
            )
        {
            string value;
            return (environmenVariables.TryGetValue(key, out value) &&
                !string.IsNullOrWhiteSpace(value))
                ? value
                : null;
        }

        internal static int? TryParseInt(
            this IDictionary<string, string> environmenVariables,
            string key
            )
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
