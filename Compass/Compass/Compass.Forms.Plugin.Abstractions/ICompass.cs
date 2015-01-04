using System;

namespace Compass.Forms.Plugin.Abstractions
{
    /// <summary>
    /// Compass Interface
    /// </summary>
    public interface ICompass
    {
        bool IsSupported();
        void Start();
        void Stop();

        event EventHandler<CompassDataChangedEventArgs> DirectionChanged;
    }
}
