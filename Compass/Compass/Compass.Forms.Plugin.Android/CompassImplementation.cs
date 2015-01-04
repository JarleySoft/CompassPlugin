using Compass.Forms.Plugin.Abstractions;
using System;
using Xamarin.Forms;
using Compass.Forms.Plugin.Droid;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Hardware;
using Android.OS;
using Android.Views;

[assembly: Dependency(typeof(CompassImplementation))]
namespace Compass.Forms.Plugin.Droid
{
    /// <summary>
    /// Compass Implementation
    /// </summary>
    public class CompassImplementation : Java.Lang.Object, ICompass, ISensorEventListener
    {
        private SensorManager mSensorManager;
        private Sensor mSensor;

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
            var ctx = Xamarin.Forms.Forms.Context;

            if (ctx != null)
            {
                mSensorManager = (SensorManager)ctx.GetSystemService(Context.SensorService);
                mSensor = mSensorManager.GetDefaultSensor(SensorType.Orientation);

                mSensorManager.RegisterListener(this, mSensor, SensorDelay.Fastest);

            }

        }

        public void Stop()
        {

        }

        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {
            
        }

        public void OnSensorChanged(SensorEvent e)
        {
            if (e.Values.Count > 0)
            {
                if (DirectionChanged != null)
                    DirectionChanged(this, new CompassDataChangedEventArgs { Heading = Convert.ToDouble(e.Values[0]) });
            }

        }

    }
}
