using System;

using Microsoft.Kinect;

namespace PresentPerfect.Detector
{
    public interface IDetector
    {
        event Action<string> Detected;

        void Track(Skeleton skeleton);
    }
}