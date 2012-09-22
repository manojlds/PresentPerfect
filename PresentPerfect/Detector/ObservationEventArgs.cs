using System;
using System.Windows.Media.Imaging;

namespace PresentPerfect.Detector
{
    public class ObservationEventArgs : EventArgs
    {
        public IObservation Observation { get; set; }
        public WriteableBitmap Image { get; set; }
    }
}