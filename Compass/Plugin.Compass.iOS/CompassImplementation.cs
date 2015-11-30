using Plugin.Compass.Abstractions;
using System;


#if __UNIFIED__
using CoreLocation;
using Foundation;
#else
using MonoTouch.CoreLocation;
using MonoTouch.Foundation;
#endif

namespace Plugin.Compass
{
    /// <summary>
    /// Compass Implementation
    /// </summary>
    public class CompassImplementation : BaseCompass
    {
        static CLLocationManager locationManager;

        /// <summary>
        /// Start tracking changes
        /// </summary>
        public override void Start()
        {
            if (locationManager == null)
            {
                locationManager = new CLLocationManager();
                locationManager.DesiredAccuracy = CLLocation.AccuracyBest;
                locationManager.HeadingFilter = 1;
                locationManager.UpdatedHeading += LocationManager_UpdatedHeading;
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

#if __UNIFIED__
            Invoker.BeginInvokeOnMainThread(action);
#else
            Invoker.BeginInvokeOnMainThread(new NSAction(action));
#endif
        }

        /// <summary>
        /// Stop tracking changes
        /// </summary>
        public override void Stop()
        {
            if (locationManager != null)
                locationManager.StopUpdatingHeading();
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
                    locationManager.UpdatedHeading -= LocationManager_UpdatedHeading;
                    locationManager = null;
                }

                disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
