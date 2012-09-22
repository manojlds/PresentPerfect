using PresentPerfect.Detector;

namespace PresentPerfect.Posture
{
    public interface IPosture : IObservation
    {
        bool IsDetected(SkeletonVector skeletonVector);
    }
}