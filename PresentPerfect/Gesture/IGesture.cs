using Microsoft.Kinect;

namespace PresentPerfect.Gesture
{
    public interface IGesture
    {
        bool IsDetected(Skeleton skeleton);

        string Name { get; }
    }
}
