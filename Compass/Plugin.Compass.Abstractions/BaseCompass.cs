

using System;

namespace Plugin.Compass.Abstractions
{
    /// <summary>
    /// Base class for compass
    /// </summary>
    public abstract class BaseCompass : ICompass, IDisposable
    {
        /// <summary>
        /// Gets if supported
        /// </summary>
        public virtual bool IsSupported => true;

        public event EventHandler<CompassChangedEventArgs> CompassChanged;


        /// <summary>
        /// When compass changes
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnCompassChanged(CompassChangedEventArgs e) =>
            CompassChanged?.Invoke(this, e);

        /// <summary>
        /// Start tracking changes
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Stop tracking changes
        /// </summary>
        public abstract void Stop();


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
        ~BaseCompass()
        {
            Dispose(false);
        }
        private bool disposed = false;
        /// <summary>
        /// Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //dispose only
                }

                disposed = true;
            }
        }
    }
}
