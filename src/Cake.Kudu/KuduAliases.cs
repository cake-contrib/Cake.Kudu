using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Kudu.Provider;

namespace Cake.Kudu
{
    // ReSharper disable once UnusedMember.Global

    /// <summary>
    /// Contains aliases related to Kudu web site environment and deployment.
    /// </summary>
    [CakeAliasCategory("Kudu")]
    public static class KuduAliases
    {
        // ReSharper disable once UnusedMember.Global

        /// <summary>
        /// Gets a <see cref="KuduProvider"/> instance that can be used to query for information and manipulate the current Kudu environment.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="KuduProvider"/> instance.</returns>
        [CakePropertyAlias(Cache = true)]
        [CakeNamespaceImport("Cake.Kudu.Provider")]
        [CakeNamespaceImport("Cake.Kudu.KuduSync")]
        public static KuduProvider Kudu(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return new KuduProvider(context);
        }
    }
}