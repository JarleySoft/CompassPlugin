using Plugin.Compass.Abstractions;
using System;

namespace Plugin.Compass
{
    /// <summary>
    /// Cross platform Compass implementations
    /// </summary>
    public static class CrossCompass
    {
        static Lazy<ICompass> Implementation = new Lazy<ICompass>(() => CreateCompass(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Current settings to use
        /// </summary>
        public static ICompass Current
        {
            get
            {
                var ret = Implementation.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

        static ICompass CreateCompass()
        {
#if PORTABLE
            return null;
#else
            return new CompassImplementation();
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }

        /// <summary>
        /// Dispose of everything 
        /// </summary>
        public static void Dispose()
        {
            if (Implementation?.IsValueCreated ?? false)
            {
                Implementation.Value.Dispose();

                Implementation = new Lazy<ICompass>(() => CreateCompass(), System.Threading.LazyThreadSafetyMode.PublicationOnly);
            }
        }
    }
}
