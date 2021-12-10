using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Kudu.Helpers;

namespace Cake.Kudu.Provider
{
    /// <summary>
    /// Provides info about tools in the Kudu environment.
    /// </summary>
    public class Tools
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Tools"/> class.
        /// </summary>
        /// <param name="environmenVariables">The environmenVariables.</param>
        internal Tools(IDictionary<string, string> environmenVariables)
        {
            KuduSyncCmd = environmenVariables.TryGetString("KUDU_SYNC_CMD");
            MSBuild = environmenVariables.TryParseFilePath("MSBUILD_PATH");
            NPMJs = environmenVariables.TryParseFilePath("NPM_JS_PATH");
            NuGet = environmenVariables.TryParseFilePath("NUGET_EXE");
        }

        // ReSharper disable MemberCanBePrivate.Global
        // ReSharper disable UnusedAutoPropertyAccessor.Global

        /// <summary>
        /// Gets the console command used for executing KuduSync (folder sync utility).
        /// </summary>
        public string KuduSyncCmd { get; }

        /// <summary>
        /// Gets path to MSBuild.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public FilePath MSBuild { get; }

        /// <summary>
        /// Gets path NPM js.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public FilePath NPMJs { get; }

        /// <summary>
        /// Gets path to NuGet.exe.
        /// </summary>
        public FilePath NuGet { get; }

        // ReSharper restore MemberCanBePrivate.Global
        // ReSharper restore UnusedAutoPropertyAccessor.Global
    }
}