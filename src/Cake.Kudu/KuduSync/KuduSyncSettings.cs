﻿using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Kudu.KuduSync
{
    /// <summary>
    /// Contains settings used by <see cref="KuduSyncRunner"/>.
    /// </summary>
    public sealed class KuduSyncSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets path to next manifest.
        /// </summary>
        public FilePath NextManifest { get; set; }

        /// <summary>
        /// Gets or sets path to previous / current manifest.
        /// </summary>
        public FilePath PreviousManifest { get; set; }

        /// <summary>
        /// Gets or sets paths to ignore.
        /// </summary>
        // ReSharper disable once CollectionNeverUpdated.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public ICollection<string> PathsToIgnore { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether manifests should be ignored.
        /// </summary>
        public bool IgnoreManifest { get; set; }
    }
}
