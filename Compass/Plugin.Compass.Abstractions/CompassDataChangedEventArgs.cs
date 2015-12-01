using System;

namespace Plugin.Compass.Abstractions
{
    /// <summary>
    /// Compass event args
    /// </summary>
    public class CompassChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="heading"></param>
        public CompassChangedEventArgs(double heading)
        {
            Heading = heading;
        }
        /// <summary>
        /// Heading
        /// </summary>
        public double Heading { get; }
    }
}
