using Compass.Forms.Plugin.Abstractions;
using System;
using Xamarin.Forms;
using Compass.Forms.Plugin.WindowsPhone;

using Microsoft.Devices.Sensors;
using System.Windows.Threading;
using Microsoft.Xna.Framework;

[assembly: Dependency(typeof(CompassImplementation))]
namespace Compass.Forms.Plugin.WindowsPhone
{
    /// <summary>
    /// Compass Implementation
    /// </summary>
    public class CompassImplementation : ICompass
    {
        Microsoft.Devices.Sensors.Compass compass;

        double magneticHeading;
        double trueHeading;
        double headingAccuracy;

        Vector3 rawMagnetometerReading;
        bool isDataValid;

        bool calibrating = false;

        /// <summary>
        /// Used for registration with dependency service
        /// </summary>
        public static void Init() { }

        public bool IsSupported()
        {
            return Microsoft.Devices.Sensors.Compass.IsSupported;
        }

        public event EventHandler<CompassDataChangedEventArgs> DirectionChanged;

        public void Start()
        {
            if (Microsoft.Devices.Sensors.Compass.IsSupported)
            {
                if (compass == null)
                {
                    // Instantiate the compass.
                    compass = new Microsoft.Devices.Sensors.Compass();
                    compass.TimeBetweenUpdates = TimeSpan.FromMilliseconds(20);

                    compass.CurrentValueChanged +=
                        new EventHandler<SensorReadingEventArgs<CompassReading>>(compass_CurrentValueChanged);
                    compass.Calibrate +=
                        new EventHandler<CalibrationEventArgs>(compass_Calibrate);

                    try
                    {
                        compass.Start();
                    }
                    catch (InvalidOperationException e)
                    {

                    }
                }
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private void compass_Calibrate(object sender, CalibrationEventArgs e)
        {
            // Should implement this at some point...
        }

        private void compass_CurrentValueChanged(object sender, SensorReadingEventArgs<CompassReading> e)
        {
            isDataValid = compass.IsDataValid;

            trueHeading = e.SensorReading.TrueHeading;
            magneticHeading = e.SensorReading.MagneticHeading;
            headingAccuracy = Math.Abs(e.SensorReading.HeadingAccuracy);
            rawMagnetometerReading = e.SensorReading.MagnetometerReading;

            if (DirectionChanged != null)
                DirectionChanged(this, new CompassDataChangedEventArgs { Heading = e.SensorReading.TrueHeading });
        }

        public void Stop()
        {
            if (compass != null && isDataValid)
            {
                compass.Stop();
            }
        }
    }
}
