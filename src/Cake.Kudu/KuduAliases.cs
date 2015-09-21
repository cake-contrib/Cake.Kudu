using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Kudu.Provider;

namespace Cake.Kudu
{
    /// <summary>
    /// Contains aliases related to Kudu web site evironment and deployment
    /// </summary>
    [CakeAliasCategory("Kudu")]
    // ReSharper disable once UnusedMember.Global
    public static class KuduAliases
    {
        /// <summary>
        /// Gets a <see cref="KuduProvider"/> instance that can be used to query for information and manipulate the current Kudu environment
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="KuduProvider"/> instance.</returns>
        [CakePropertyAlias(Cache = true)]
        [CakeNamespaceImport("Cake.Kudu.Provider")]
        // ReSharper disable once UnusedMember.Global
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