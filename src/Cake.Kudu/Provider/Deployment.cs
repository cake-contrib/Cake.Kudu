using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Kudu.Helpers;

namespace Cake.Kudu.Provider
{
    /// <summary>
    /// Provides info about Kudu deployment environment.
    /// </summary>
    public sealed class Deployment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Deployment"/> class.
        /// </summary>
        /// <param name="environmenVariables">The environmenVariables.</param>
        internal Deployment(IDictionary<string, string> environmenVariables)
        {
            Command = environmenVariables.TryGetString("command");
            Source = environmenVariables.TryParseDirectoryPath("DEPLOYMENT_SOURCE");
            Target = environmenVariables.TryParseDirectoryPath("DEPLOYMENT_TARGET");
            Temp = environmenVariables.TryParseDirectoryPath("DEPLOYMENT_TEMP");
            NextManifest = environmenVariables.TryParseFilePath("NEXT_MANIFEST_PATH");
            PreviousManifest = environmenVariables.TryParseFilePath("PREVIOUS_MANIFEST_PATH");
        }

        // ReSharper disable MemberCanBePrivate.Global
        // ReSharper disable UnusedAutoPropertyAccessor.Global

        /// <summary>
        /// Gets the command that triggered deployment.
        /// </summary>
        public string Command { get; }

        /// <summary>
        /// Gets the deployment source path (if SCM driven it's path to the repository).
        /// </summary>
        public DirectoryPath Source { get; }

        /// <summary>
        /// Gets the deployment target path, build output artifacts goes here.
        /// </summary>
        public DirectoryPath Target { get; }

        /// <summary>
        /// Gets the deployment temporary path, use this for temporary assets used in build process.
        /// </summary>
        public DirectoryPath Temp { get; }

        /// <summary>
        /// Gets the KuduSync next manifest path.
        /// </summary>
        public FilePath NextManifest { get; }

        /// <summary>
        /// Gets the KuduSync previous / current manifest path.
        /// </summary>
        public FilePath PreviousManifest { get; }

        // ReSharper restore MemberCanBePrivate.Global
        // ReSharper restore UnusedAutoPropertyAccessor.Global
    }
}