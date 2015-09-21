using System.Collections.Generic;
using Cake.Kudu.Helpers;

namespace Cake.Kudu.Provider
{
    /// <summary>
    /// Provides info about Kudu SCM environment
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public sealed class SCM
    {
        // ReSharper disable MemberCanBePrivate.Global
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        /// <summary>
        /// Gets the source control build args.
        /// </summary>
        public string BuildArgs { get; }

        /// <summary>
        /// Gets the source control commands idle timeout.
        /// </summary>
        public int? CommandIdleTimeout { get; }

        /// <summary>
        /// Gets the commit id that triggered deployment.
        /// </summary>
        public string CommitId { get; }

        /// <summary>
        /// Gets the email of current SCM user.
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// Gets the current SCM user name.
        /// </summary>
        public string UserName { get; }

        /// <summary>
        /// Gets the type of SCM system, i.e. GitHub.
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Gets the SCM branch that triggered the deployment.
        /// </summary>
        public string Branch { get; }
        // ReSharper restore MemberCanBePrivate.Global
        // ReSharper restore UnusedAutoPropertyAccessor.Global

        internal SCM(IDictionary<string, string> environmenVariables)
        {
            BuildArgs = environmenVariables.TryGetString("SCM_BUILD_ARGS");
            CommandIdleTimeout = environmenVariables.TryParseInt("SCM_COMMAND_IDLE_TIMEOUT");
            CommitId = environmenVariables.TryGetString("SCM_COMMIT_ID");
            Email = environmenVariables.TryGetString("SCM_GIT_EMAIL");
            UserName = environmenVariables.TryGetString("SCM_GIT_USERNAME");
            Type = environmenVariables.TryGetString("ScmType");
            Branch = environmenVariables.TryGetString("deployment_branch");
        }
    }
}