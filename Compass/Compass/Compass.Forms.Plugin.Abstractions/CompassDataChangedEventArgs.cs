using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compass.Forms.Plugin.Abstractions
{
    public class CompassDataChangedEventArgs : EventArgs
    {
        public double Heading { get; set; }
    }
}
