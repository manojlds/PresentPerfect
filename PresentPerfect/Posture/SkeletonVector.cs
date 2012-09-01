using System.Collections.Generic;
using System.Linq;

using Kinect.Toolbox;

using Microsoft.Kinect;

namespace PresentPerfect.Posture
{
    public class SkeletonVector
    {
        public Vector3? HeadPosition { get; private set; }
        public Vector3? LeftHandPosition { get; private set; }
        public Vector3? RightHandPosition { get; private set; }

        public SkeletonVector(IEnumerable<Joint> joints)
        {
            foreach (var joint in joints.Where(joint => joint.TrackingState == JointTrackingState.Tracked))
            {
                switch (joint.JointType)
                {
                    case JointType.Head:
                        HeadPosition = joint.Position.ToVector3();
                        break;
                    case JointType.HandLeft:
                        LeftHandPosition = joint.Position.ToVector3();
                        break;
                    case JointType.HandRight:
                        RightHandPosition = joint.Position.ToVector3();
                        break;
                }
            }
        }
    }
}