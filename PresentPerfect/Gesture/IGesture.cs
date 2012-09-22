using Microsoft.Kinect;
using PresentPerfect.Detector;

namespace PresentPerfect.Gesture
{
    public interface IGesture : IObservation
    {
        bool IsDetected(Skeleton skeleton);
        
    }
}
