using System;
using Kinect.Toolbox;

namespace PresentPerfect.Posture
{
    public class HandsOverHeadPosture : IPosture
    {
        private const float MaxRange = 0.25f;

        public bool IsDetected(SkeletonVector skeletonVector)
        {
            if (CheckHandOverHead(skeletonVector.HeadPosition, skeletonVector.LeftHandPosition))
            {
                Name = "LeftHandOverHead";
                return true;
            }

            if (CheckHandOverHead(skeletonVector.HeadPosition, skeletonVector.RightHandPosition))
            {
                Name = "RightHandsOverHead";
                return true;
            }

            return false;
        }

        private static bool CheckHandOverHead(Vector3? headPosition, Vector3? handPosition)
        {
            if (!handPosition.HasValue || !headPosition.HasValue)
                return false;

            if (handPosition.Value.Y < headPosition.Value.Y)
                return false;

            if (Math.Abs(handPosition.Value.X - headPosition.Value.X) > MaxRange)
                return false;

            if (Math.Abs(handPosition.Value.Z - headPosition.Value.Z) > MaxRange)
                return false;

            return true;
        }

        public string Name { get; private set; }
    }
}
