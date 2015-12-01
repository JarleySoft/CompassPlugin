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
        public override void Start()
        {
            if (!IsSupported)
                return;

            if (compass == null)
            {
                // Instantiate the compass.
                compass = Windows.Devices.Sensors.Compass.GetDefault();
                compass.ReportInterval = compass.MinimumReportInterval >= 20 ? 20 : compass.MinimumReportInterval;
            }


            compass.ReadingChanged += Compass_ReadingChanged;
        }

        private async void Compass_ReadingChanged(Windows.Devices.Sensors.Compass sender, Windows.Devices.Sensors.CompassReadingChangedEventArgs args)
        {
            if (args?.Reading?.HeadingTrueNorth.HasValue ?? false)
            {
#if SILVERLIGHT
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                     OnCompassChanged(new CompassChangedEventArgs(args.Reading.HeadingTrueNorth.Value));
                });
#else

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

               
#endif
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
