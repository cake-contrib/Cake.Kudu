using System;
using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Kudu.Helpers;

namespace Cake.Kudu.Provider
{
    /// <summary>
    /// Provides info about tools in the Kudu environment
    /// </summary>
    public class Tools
    {// ReSharper disable MemberCanBePrivate.Global
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        /// <summary>
        /// Console command used for executing KuduSync (folder sync utility).
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
        /// Gets path to NuGet.exe
        /// </summary>
        public FilePath NuGet { get; }

        /// <summary>
        /// Gets path to the .NET SDK Manager PowerShell script.
        /// </summary>
        [Obsolete(".NET Core replaced DNV, this property will be removed in next version.", false)]
        // ReSharper disable once InconsistentNaming
        public FilePath DNVMPs { get; }
        // ReSharper restore MemberCanBePrivate.Global
        // ReSharper restore UnusedAutoPropertyAccessor.Global


        internal Tools(IDictionary<string, string> environmenVariables)
        {
            KuduSyncCmd = environmenVariables.TryGetString("KUDU_SYNC_CMD");
            MSBuild = environmenVariables.TryParseFilePath("MSBUILD_PATH");
            NPMJs = environmenVariables.TryParseFilePath("NPM_JS_PATH");
            NuGet = environmenVariables.TryParseFilePath("NUGET_EXE");
#pragma warning disable 618
            DNVMPs = environmenVariables.TryParseFilePath("SCM_DNVM_PS_PATH");
#pragma warning restore 618
        }
    }
}