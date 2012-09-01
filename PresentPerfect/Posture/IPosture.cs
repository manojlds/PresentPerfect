namespace PresentPerfect.Posture
{
    public interface IPosture
    {
        bool IsDetected(SkeletonVector skeletonVector);

        string Name { get; }
    }
}