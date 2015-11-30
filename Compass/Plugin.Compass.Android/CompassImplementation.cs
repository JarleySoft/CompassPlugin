using Plugin.Compass.Abstractions;
using System;

using Android.App;
using Android.Content;
using Android.Hardware;
using System.Linq;

namespace Plugin.Compass
{
    /// <summary>
    /// Compass Implementation
    /// </summary>
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

        bool listenting;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CompassImplementation()
        {
            Init();
        }

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
            get { Init(); return sensorManager != null && accelerometer != null && magnetometer != null; }
        }

        /// <summary>
        /// Start tracking
        /// </summary>
        public void Start()
        {
            if (listenting)
            {
                System.Diagnostics.Debug.WriteLine("Already Listening.");
                return;
            }

            if (!IsSupported)
            {
                System.Diagnostics.Debug.WriteLine("Not supported on this device.");
                return;
            }

            listenting = true;


            sensorManager.RegisterListener(this, accelerometer, SensorDelay.Game);
            sensorManager.RegisterListener(this, magnetometer, SensorDelay.Game);


        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            if (!listenting)
                return;

            listenting = false;

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
                    Array.Copy(e.Values.ToArray(), 0, lastAccelerometer, 0, e.Values.Count);
                    lastAccelerometerSet = true;
                }
                else if (e.Sensor == magnetometer && !lastMagnetometerSet)
                {
                    Array.Copy(e.Values.ToArray(), 0, lastMagnetometer, 0, e.Values.Count);
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
                    sensorManager = null;
                    accelerometer = null;
                    magnetometer = null;
                }

                disposed = true;
            }
        }
    }
}
