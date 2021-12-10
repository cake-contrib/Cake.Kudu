using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Common;
using Cake.Core;
using Cake.Core.IO;
using Cake.Kudu.KuduSync;

namespace Cake.Kudu.Provider
{
    /// <summary>
    /// Exposes functionality related to Kudu environment.
    /// </summary>
    public sealed class KuduProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KuduProvider"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        internal KuduProvider(ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var environmenVariables = context.EnvironmentVariables();
            Deployment = new Deployment(environmenVariables);
            WebSite = new WebSite(environmenVariables);
            SCM = new SCM(environmenVariables);
            Tools = new Tools(environmenVariables);
            AppSettings = environmenVariables
                .Where(key => key.Key.StartsWith("APPSETTING_"))
                .ToDictionary(
                    key => string.Concat(key.Key.Skip(11)),
                    value => value.Value);

            ConnectionStrings = environmenVariables
                .Where(key => key.Key.StartsWith("SQLAZURECONNSTR_"))
                .ToDictionary(
                    key => string.Concat(key.Key.Skip(16)),
                    value => value.Value);

            IsRunningOnKudu = !string.IsNullOrWhiteSpace(WebSite.Name);

            KuduSyncRunner = new KuduSyncRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
        }

        private KuduSyncRunner KuduSyncRunner { get; }

        // ReSharper disable MemberCanBePrivate.Global
        // ReSharper disable UnusedAutoPropertyAccessor.Global

        /// <summary>
        /// Gets a value indicating whether the current build is running on Kudu.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on Kudu.; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunningOnKudu { get; }

        /// <summary>
        /// Gets the Kudu deployment environment.
        /// </summary>
        public Deployment Deployment { get; }

        /// <summary>
        /// Gets the Kudu website environment.
        /// </summary>
        public WebSite WebSite { get; }

        /// <summary>
        /// Gets the Kudu source control environment.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public SCM SCM { get; }

        /// <summary>
        /// Gets the Kudu tools environment.
        /// </summary>
        public Tools Tools { get; }

        /// <summary>
        /// Gets the Kudu app settings.
        /// </summary>
        public IDictionary<string, string> AppSettings { get; }

        /// <summary>
        /// Gets the Kudu connection strings.
        /// </summary>
        public IDictionary<string, string> ConnectionStrings { get; }

        // ReSharper restore MemberCanBePrivate.Global
        // ReSharper restore UnusedAutoPropertyAccessor.Global

        /// <summary>
        /// Sync two folders content.
        /// </summary>
        /// <param name="source">The source directory path.</param>
        // ReSharper disable once UnusedMember.Global
        public void Sync(DirectoryPath source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            Sync(source, Deployment.Target);
        }

        /// <summary>
        /// Sync two folders content.
        /// </summary>
        /// <param name="source">The source directory path.</param>
        /// <param name="target">The target directory path.</param>
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public void Sync(DirectoryPath source, DirectoryPath target)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            Sync(source, target, new KuduSyncSettings
            {
                PreviousManifest = Deployment.PreviousManifest,
                NextManifest = Deployment.NextManifest,
            });
        }

        /// <summary>
        /// Sync two folders content.
        /// </summary>
        /// <param name="source">The source directory path.</param>
        /// <param name="target">The target directory path.</param>
        /// <param name="settings">The settings.</param>
        // ReSharper disable once MemberCanBePrivate.Global
        public void Sync(DirectoryPath source, DirectoryPath target, KuduSyncSettings settings)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            KuduSyncRunner.Sync(source, target, settings);
        }
    }
}