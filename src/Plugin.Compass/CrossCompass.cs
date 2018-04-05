using Plugin.Compass.Abstractions;
using System;

namespace Plugin.Compass
{
    /// <summary>
    /// Cross platform Compass implementations
    /// </summary>
    public static class CrossCompass
    {
        static Lazy<ICompass> implementation = new Lazy<ICompass>(() => CreateCompass(), System.Threading.LazyThreadSafetyMode.PublicationOnly);
        /// <summary>
        /// Gets if the plugin is supported on the current platform.
        /// </summary>
        public static bool IsSupported => implementation.Value?.IsSupported ?? false;

        /// <summary>
        /// Current plugin implementation to use
        /// </summary>
        public static ICompass Current
        {
            get
            {
                var ret = implementation.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

        static ICompass CreateCompass()
        {
#if NETSTANDARD1_0
            return null;
#else
            return new CompassImplementation();
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly() =>
            new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");


        /// <summary>
        /// Dispose of everything 
        /// </summary>
        public static void Dispose()
        {
            if (implementation?.IsValueCreated ?? false)
            {
                implementation.Value.Dispose();

                implementation = new Lazy<ICompass>(() => CreateCompass(), System.Threading.LazyThreadSafetyMode.PublicationOnly);
            }
        }
    }
}
