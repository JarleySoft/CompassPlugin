using System;

namespace Plugin.Compass.Abstractions
{
    /// <summary>
    /// Compass Interface
    /// </summary>
    public interface ICompass : IDisposable
    {
        /// <summary>
        /// Is compass supported on this device
        /// </summary>
        bool IsSupported { get; }

        /// <summary>
        /// Start tracking changes
        /// </summary>
        void Start();

        /// <summary>
        /// Stop tracking changes
        /// </summary>
        void Stop();

        /// <summary>
        /// Triggered with compass changes
        /// </summary>
        event EventHandler<CompassChangedEventArgs> CompassChanged;
    }
}
