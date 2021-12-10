using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Kudu.Helpers;

namespace Cake.Kudu.Provider
{
    /// <summary>
    /// Provides info about Kudu website environment.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public sealed class WebSite
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebSite"/> class.
        /// </summary>
        /// <param name="environmenVariables">The environmenVariables.</param>
        internal WebSite(IDictionary<string, string> environmenVariables)
        {
            WebRoot = environmenVariables.TryParseDirectoryPath("WEBROOT_PATH");
            AuthEnabled = environmenVariables.TryParseBool("WEBSITE_AUTH_ENABLED");
            ComputeMode = environmenVariables.TryGetString("WEBSITE_COMPUTE_MODE");
            HostName = environmenVariables.TryGetString("WEBSITE_HOSTNAME");
            HttpLoggingEnabled = environmenVariables.TryParseBool("WEBSITE_HTTPLOGGING_ENABLED");
            IISSiteName = environmenVariables.TryGetString("WEBSITE_IIS_SITE_NAME");
            InstanceId = environmenVariables.TryGetString("WEBSITE_INSTANCE_ID");
            NodeDefaulVersion = environmenVariables.TryGetString("WEBSITE_NODE_DEFAULT_VERSION");
            OwnerName = environmenVariables.TryGetString("WEBSITE_HOSTNAMEEBSITE_OWNER_NAME");
            AlwaysOnEnabled = environmenVariables.TryParseBool("WEBSITE_SCM_ALWAYS_ON_ENABLED");
            Mode = environmenVariables.TryGetString("WEBSITE_SITE_MODE");
            Name = environmenVariables.TryGetString("WEBSITE_SITE_NAME");
            SKU = environmenVariables.TryGetString("WEBSITE_SKU");
            Region = environmenVariables.TryGetString("REGION_NAME");
        }

        // ReSharper disable MemberCanBePrivate.Global
        // ReSharper disable UnusedAutoPropertyAccessor.Global

        /// <summary>
        /// Gets the path to current IIS web root.
        /// </summary>
        public DirectoryPath WebRoot { get; }

        /// <summary>
        /// Gets if authentication is enabled.
        /// </summary>
        public bool? AuthEnabled { get; }

        /// <summary>
        /// Gets the web site compute mode, i.e. Shared.
        /// </summary>
        public string ComputeMode { get; }

        /// <summary>
        /// Gets the web host name.
        /// </summary>
        public string HostName { get; }

        /// <summary>
        /// Gets if http logging is specified and if it's enabled or not.
        /// </summary>
        public bool? HttpLoggingEnabled { get; }

        /// <summary>
        /// Gets the IIS internal site name.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string IISSiteName { get; }

        /// <summary>
        /// Gets the internal ID of current instance.
        /// </summary>
        public string InstanceId { get; }

        /// <summary>
        /// Gets the default version of Node.
        /// </summary>
        public string NodeDefaulVersion { get; }

        /// <summary>
        /// Gets the site instance owner name.
        /// </summary>
        public string OwnerName { get; }

        /// <summary>
        /// Gets if always on is specified and if it's enabled or not.
        /// </summary>
        public bool? AlwaysOnEnabled { get; }

        /// <summary>
        /// Gets site mode i.e. Limited.
        /// </summary>
        public string Mode { get; }

        /// <summary>
        /// Gets the name of the site.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the current site SKU, i.e. Free.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string SKU { get; }

        /// <summary>
        /// Gets the region of the site, i.e. "North Europe".
        /// </summary>
        public string Region { get; }

        // ReSharper restore MemberCanBePrivate.Global
        // ReSharper restore UnusedAutoPropertyAccessor.Global
    }
}