namespace PresentPerfect.Detector
{
    public interface IObservation
    {
        bool IsPositive { get; }
        string Name { get; }
    }
}