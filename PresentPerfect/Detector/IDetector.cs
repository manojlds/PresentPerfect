using System;

using Microsoft.Kinect;

namespace PresentPerfect.Detector
{
    public interface IDetector
    {
        event Action<IObservation> Detected;
        void Track(Skeleton skeleton);
    }
}