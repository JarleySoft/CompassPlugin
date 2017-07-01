using Plugin.Compass.Abstractions;
using System;

using Android.App;
using Android.Content;
using Android.Hardware;
using System.Linq;
using Android.Runtime;

namespace Plugin.Compass
{
    /// <summary>
    /// Compass Implementation
    /// </summary>
    [Preserve(AllMembers = true)]
    public class CompassImplementation : Java.Lang.Object, ICompass, IDisposable, ISensorEventListener
    {
        SensorManager sensorManager;
        Sensor accelerometer;
        Sensor magnetometer;
        float[] lastAccelerometer = new float[3];
        float[] lastMagnetometer = new float[3];
        bool lastAccelerometerSet;
        bool lastMagnetometerSet;
        float[] r = new float[9];
        float[] orientation = new float[3];

        bool listening;

        void Init()
        {

            var ctx = Application.Context;
            if (ctx == null)
            {
                System.Diagnostics.Debug.WriteLine("Context not found, can not start.");
                return;
            }
            if (sensorManager == null)
                sensorManager = ctx.GetSystemService(Context.SensorService) as SensorManager;


            if (accelerometer == null)
                accelerometer = sensorManager?.GetDefaultSensor(SensorType.Accelerometer);

            if (magnetometer == null)
                magnetometer = sensorManager?.GetDefaultSensor(SensorType.MagneticField);

        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSupported
        {
            get 
            {
                Init(); 
                return sensorManager != null && accelerometer != null && magnetometer != null; 
            }
        }

        /// <summary>
        /// Start tracking
        /// </summary>
        public void Start(SensorSpeed speed = SensorSpeed.Normal)
        {
            if (listening)
            {
                System.Diagnostics.Debug.WriteLine("Already Listening.");
                return;
            }

            //this will call Init() to initialize
            if (!IsSupported)
            {
                System.Diagnostics.Debug.WriteLine("Not supported on this device.");
                return;
            }

            listening = true;


            var delay = SensorDelay.Normal;
            switch(speed)
            {
                case SensorSpeed.Normal:
                    delay = SensorDelay.Normal;
                    break;
                case SensorSpeed.Fastest:
                    delay = SensorDelay.Fastest;
                    break;
                case SensorSpeed.Game:
                    delay = SensorDelay.Game;
                    break;
                case SensorSpeed.UI:
                    delay = SensorDelay.Ui;
                    break;
            }

            sensorManager.RegisterListener(this, accelerometer, delay);
            sensorManager.RegisterListener(this, magnetometer, delay);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            if (!listening)
                return;

            listening = false;

            if (accelerometer != null)
                sensorManager?.UnregisterListener(this, accelerometer);

            if (magnetometer != null)
                sensorManager?.UnregisterListener(this, magnetometer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sensor"></param>
        /// <param name="accuracy"></param>
        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<CompassChangedEventArgs> CompassChanged;


        /// <summary>
        /// When connectivity changes
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnCompassChanged(CompassChangedEventArgs e) =>
            CompassChanged?.Invoke(this, e);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public void OnSensorChanged(SensorEvent e)
        {
            lock(locker)
            {
                if (e.Sensor == accelerometer && !lastAccelerometerSet)
                {
                    CopyValues (e.Values, lastAccelerometer);
                    lastAccelerometerSet = true;
                }
                else if (e.Sensor == magnetometer && !lastMagnetometerSet)
                {
                    CopyValues (e.Values, lastMagnetometer);
                    lastMagnetometerSet = true;
                }

                if (lastAccelerometerSet && lastMagnetometerSet)
                {
                    SensorManager.GetRotationMatrix(r, null, lastAccelerometer, lastMagnetometer);
                    SensorManager.GetOrientation(r, orientation);
                    var azimuthInRadians = orientation[0];
                    var azimuthInDegress = (Java.Lang.Math.ToDegrees(azimuthInRadians) + 360.0) % 360.0;


                    OnCompassChanged(new CompassChangedEventArgs(azimuthInDegress));
                    lastMagnetometerSet = false;
                    lastAccelerometerSet = false;
                }
            }
        }

        void CopyValues(System.Collections.Generic.IList<float> source, float[] destination) 
        {
            for (int i = 0; i < source.Count; ++i) {
                destination [i] = source [i];
            }
        }

        object locker = new object();


        /// <summary>
        /// Dispose of class and parent classes
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose up
        /// </summary>
        ~CompassImplementation()
        {
            Dispose(false);
        }
        private bool disposed = false;
        /// <summary>
        /// Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        public void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Stop();
                    sensorManager?.Dispose();
                    sensorManager = null;
                    accelerometer?.Dispose();
                    accelerometer = null;
                    magnetometer?.Dispose();
                    magnetometer = null;
                }

                disposed = true;
            }
        }
    }
}
