using Plugin.Compass.Abstractions;
using System;
using CoreLocation;
using Foundation;

namespace Plugin.Compass
{
    /// <summary>
    /// Compass Implementation
    /// </summary>
    [Preserve(AllMembers = true)]
    public class CompassImplementation : BaseCompass
    {
        static CLLocationManager locationManager;


        public override bool IsSupported =>
            CLLocationManager.HeadingAvailable;

        void Init()
        {
            if (locationManager != null)
                return;
            
            locationManager = new CLLocationManager();
            locationManager.DesiredAccuracy = CLLocation.AccuracyBest;
            locationManager.HeadingFilter = 1;
        }

        bool listening;
        /// <summary>
        /// Start tracking changes
        /// </summary>
        public override void Start(SensorSpeed speed = SensorSpeed.Normal)
        {
            if (!IsSupported)
                return;
            
            if (listening)
                return;
            
            listening = true;

            Init();
            locationManager.UpdatedHeading += LocationManager_UpdatedHeading;

            switch(speed)
            {
                case SensorSpeed.Fastest:
                    locationManager.HeadingFilter = .1;
                    locationManager.DesiredAccuracy = CLLocation.AccurracyBestForNavigation;
                    break;
                case SensorSpeed.Game:
                    locationManager.HeadingFilter = .5;
                    locationManager.DesiredAccuracy = CLLocation.AccurracyBestForNavigation;
                    break;
                case SensorSpeed.Normal:
                    locationManager.HeadingFilter = 1;
                    locationManager.DesiredAccuracy = CLLocation.AccuracyBest;
                    break;
                case SensorSpeed.UI:
                    locationManager.HeadingFilter = 1.5;
                    locationManager.DesiredAccuracy = CLLocation.AccuracyBest;
                    break;
            }

            locationManager.StartUpdatingHeading();
        }

        private void LocationManager_UpdatedHeading(object sender, CLHeadingUpdatedEventArgs e) =>
            EnsureInvokedOnMainThread(()=>OnCompassChanged(new CompassChangedEventArgs(e.NewHeading.TrueHeading)));


        static NSObject Invoker;
        static void EnsureInvokedOnMainThread(Action action)
        {
            if (NSThread.Current.IsMainThread)
            {
                action();
                return;
            }
            if (Invoker == null)
                Invoker = new NSObject();
            
            Invoker.BeginInvokeOnMainThread(action);
        }

        /// <summary>
        /// Stop tracking changes
        /// </summary>
        public override void Stop()
        {
            if (!listening)
                return;

            listening = false;

            locationManager.UpdatedHeading -= LocationManager_UpdatedHeading;

            locationManager?.StopUpdatingHeading();
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
                    locationManager?.Dispose();
                    locationManager = null;
                }

                disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
