using System;
using Plugin.Compass.Abstractions;
using Tizen.Sensor;

namespace Plugin.Compass
{
    public class CompassImplementation : BaseCompass
    {
        Lazy<OrientationSensor> _sensor = new Lazy<OrientationSensor>(() => new OrientationSensor());
        bool _isStarted = false;

        public override bool IsSupported => OrientationSensor.IsSupported;

        public override void Start(SensorSpeed speed = SensorSpeed.Normal)
        {
            if (_isStarted)
                return;

            if (!_sensor.IsValueCreated)
            {
                _sensor.Value.DataUpdated += OnChanged;
            }

            switch (speed)
            {
                case SensorSpeed.Fastest:
                    _sensor.Value.Interval = (uint)_sensor.Value.MinInterval;
                    break;
                case SensorSpeed.Game:
                    // for 60fps
                    _sensor.Value.Interval = 16;
                    break;
                case SensorSpeed.Normal:
                case SensorSpeed.UI:
                    // for 30fps
                    _sensor.Value.Interval = 33;
                    break;
            }
            _sensor.Value.Start();
            _isStarted = true;
        }

        public override void Stop()
        {
            if (!_isStarted)
                return;
            _sensor.Value.Stop();
            _isStarted = false;
        }

        public override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_sensor.IsValueCreated)
                {
                    _sensor.Value.DataUpdated -= OnChanged;
                    _sensor.Value.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        void OnChanged(object sender, OrientationSensorDataUpdatedEventArgs e)
        {
            OnCompassChanged(new CompassChangedEventArgs(e.Azimuth));
        }

    }
}
