using System;
using Kinect.Toolbox;

namespace PresentPerfect.Posture
{
    public class HandsNearHeadPosture : IPosture
    {
        private const float MaxRange = 0.20f;

        public bool IsDetected(SkeletonVector skeletonVector)
        {
            if (IsHandsNearHead(skeletonVector.HeadPosition, skeletonVector.LeftHandPosition))
            {
                Name = "LeftHandNearHead";
                return true;
            }

            if (IsHandsNearHead(skeletonVector.HeadPosition, skeletonVector.RightHandPosition))
            {
                Name = "RightHandNearHead";
                return true;
            }

            return false;
        }

        public bool IsPositive { get { return false; } }

        private static bool IsHandsNearHead(Vector3? headPosition, Vector3? handPosition)
        {
            if (!handPosition.HasValue || !headPosition.HasValue)
                return false;

            if (Math.Abs(handPosition.Value.Y - headPosition.Value.Y) > MaxRange)
                return false;

            if (Math.Abs(handPosition.Value.X - headPosition.Value.X) > MaxRange)
                return false;

            return true;
        }

        public string Name { get; private set; }
    }
}
