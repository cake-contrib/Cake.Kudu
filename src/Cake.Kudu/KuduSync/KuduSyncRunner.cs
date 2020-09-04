using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Kudu.KuduSync
{
    /// <summary>
    /// The KuduSync runner.
    /// </summary>
    public sealed class KuduSyncRunner : Tool<KuduSyncSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="KuduSyncRunner" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="toolLocator">The tool locator.</param>
        /// <param name="processRunner">The process runner.</param>
        public KuduSyncRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator toolLocator)
            : base(fileSystem, environment, processRunner, toolLocator)
        {
            _environment = environment;
        }

        /// <summary>
        /// Sync two folders content
        /// </summary>
        /// <param name="source">The source directory path.</param>
        /// <param name="target">The target directory path.</param>
        /// <param name="settings">The settings.</param>
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

            settings = settings ?? new KuduSyncSettings();

            Run(settings, GetArguments(source, target, settings));
        }

        private ProcessArgumentBuilder GetArguments(DirectoryPath source, DirectoryPath target, KuduSyncSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("--from");
            builder.AppendQuoted(source.MakeAbsolute(_environment).FullPath);

            builder.Append("--to");
            builder.AppendQuoted(target.MakeAbsolute(_environment).FullPath);

            if (settings.NextManifest != null)
            {
                builder.Append("--nextManifest");
                builder.AppendQuoted(settings.NextManifest.MakeAbsolute(_environment).FullPath);
            }

            if (settings.PreviousManifest != null)
            {
                builder.Append("--previousManifest");
                builder.AppendQuoted(settings.PreviousManifest.MakeAbsolute(_environment).FullPath);
            }

            if ((settings.NextManifest == null && settings.PreviousManifest == null) || settings.IgnoreManifest)
            {
                builder.Append("--ignoremanifest");
            }

            if (settings.PathsToIgnore?.Count > 0)
            {
                builder.Append("--ignore");
                builder.AppendQuoted(string.Join(";", settings.PathsToIgnore));
            }

            builder.Append("--perf");

            return builder;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "KuduSync";
        }

        /// <summary>
        /// Gets the name of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "KuduSync.NET.exe", "kudusync.exe", "kudusync" };
        }
    }
}
