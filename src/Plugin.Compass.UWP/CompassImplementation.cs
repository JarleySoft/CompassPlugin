using Plugin.Compass.Abstractions;
using System;

namespace Plugin.Compass
{
    /// <summary>
    /// Compass Implementation
    /// </summary>
    public class CompassImplementation : BaseCompass
    {
        Windows.Devices.Sensors.Compass compass;

        double magneticHeading;
        double trueHeading;
        double headingAccuracy;
        
        bool isDataValid;
        bool calibrating = false;
        
        /// <summary>
        /// Gets if compass is supported
        /// </summary>
        public override bool IsSupported => Windows.Devices.Sensors.Compass.GetDefault() != null;
        
        
        /// <summary>
        /// Start compass
        /// </summary>
        public override void Start(SensorSpeed speed = SensorSpeed.Normal)
        {
            if (!IsSupported)
                return;

            if (compass == null)
            {
                // Instantiate the compass.
                compass = Windows.Devices.Sensors.Compass.GetDefault();
            }

            switch(speed)
            {
                case SensorSpeed.Fastest:
                    compass.ReportInterval = compass.MinimumReportInterval >= 8 ? 8 : compass.MinimumReportInterval;
                    break;
                case SensorSpeed.Game:
                    compass.ReportInterval = compass.MinimumReportInterval >= 22 ? 22 : compass.MinimumReportInterval;
                    break;
                case SensorSpeed.UI:
                case SensorSpeed.Normal:
                    compass.ReportInterval = compass.MinimumReportInterval >= 33 ? 33 : compass.MinimumReportInterval;
                    break;
            }


            compass.ReadingChanged += Compass_ReadingChanged;
        }

        private async void Compass_ReadingChanged(Windows.Devices.Sensors.Compass sender, Windows.Devices.Sensors.CompassReadingChangedEventArgs args)
        {
            if (args?.Reading?.HeadingTrueNorth.HasValue ?? false)
            {

                var dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;
                if (dispatcher != null)
                {
                    await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        OnCompassChanged(new CompassChangedEventArgs(args.Reading.HeadingTrueNorth.Value));
                    });
                }
                else
                {
                    OnCompassChanged(new CompassChangedEventArgs(args.Reading.HeadingTrueNorth.Value));
                }
            }
        }



        /// <summary>
        /// Stop compass
        /// </summary>
        public override void Stop()
        {
            if (compass == null)
                return;

            compass.ReadingChanged -= Compass_ReadingChanged;
        }


    

        private bool disposed = false;
        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        public override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Stop();
                    compass = null;
                }

                disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
