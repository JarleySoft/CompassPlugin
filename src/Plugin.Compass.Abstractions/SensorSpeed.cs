using System;
namespace Plugin.Compass.Abstractions
{
    public enum SensorSpeed
    {
        /// <summary>
        /// Get sensor data as fast as possible
        /// </summary>
        Fastest,
        /// <summary>
        /// Rate suitable for games
        /// </summary>
        Game,
        /// <summary>
        /// Normal Rate (default) suitable for screen orientation
        /// </summary>
        Normal,
        /// <summary>
        /// Rate suitable for the User Interface
        /// </summary>
        UI
    }
}
