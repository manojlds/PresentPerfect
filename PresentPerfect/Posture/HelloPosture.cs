using System;
using Kinect.Toolbox;

namespace PresentPerfect.Posture
{
    public class HelloPosture : IPosture
    {
        private const float MaxRange = 0.25f;

        public bool IsDetected(SkeletonVector skeletonVector)
        {
            if (CheckHello(skeletonVector.HeadPosition, skeletonVector.LeftHandPosition))
            {
                Name = "LeftHandHello";
                return true;
            }

            if (CheckHello(skeletonVector.HeadPosition, skeletonVector.RightHandPosition))
            {
                Name = "RightHandHello";
            }

            return true;
        }

        private static bool CheckHello(Vector3? headPosition, Vector3? handPosition)
        {
            if (!handPosition.HasValue || !headPosition.HasValue)
                return false;

            if (Math.Abs(handPosition.Value.X - headPosition.Value.X) < MaxRange)
                return false;

            if (Math.Abs(handPosition.Value.Y - headPosition.Value.Y) > MaxRange)
                return false;

            if (Math.Abs(handPosition.Value.Z - headPosition.Value.Z) > MaxRange)
                return false;

            return true;
        }

        public string Name { get; private set; }
    }
}
