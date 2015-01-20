using Compass.Forms.Plugin.Abstractions;
using System;
using Xamarin.Forms;
using Compass.Forms.Plugin.iOS;
#if __UNIFIED__
using CoreLocation;
#else
using MonoTouch.CoreLocation;
#endif

[assembly: Dependency(typeof(CompassImplementation))]
namespace Compass.Forms.Plugin.iOS
{
    /// <summary>
    /// Compass Implementation
    /// </summary>
    public class CompassImplementation : ICompass
    {
        static CLLocationManager _locationManager;

        /// <summary>
        /// Used for registration with dependency service
        /// </summary>
        public static void Init() { }

        public event EventHandler<CompassDataChangedEventArgs> DirectionChanged;

        public bool IsSupported()
        {
            return true;
        }

        public void Start()
        {
            if (_locationManager == null)
            {
                _locationManager = new CLLocationManager();
                _locationManager.DesiredAccuracy = CLLocation.AccuracyBest;
                _locationManager.HeadingFilter = 1;

                _locationManager.UpdatedHeading += (sender, e) =>
                    {
                        double newRad = -e.NewHeading.TrueHeading;

                        if (DirectionChanged != null)
                            DirectionChanged(this, new CompassDataChangedEventArgs { Heading = e.NewHeading.TrueHeading });
                    };
            }
            _locationManager.StartUpdatingHeading();
        }

        public void Stop()
        {
            if (_locationManager != null)
                _locationManager.StopUpdatingHeading();
        }
    }
}
