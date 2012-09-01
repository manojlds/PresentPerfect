using System;
using System.Collections.Generic;
using System.Linq;
using Kinect.Toolbox;
using Microsoft.Kinect;
using PresentPerfect.Posture;

namespace PresentPerfect.Detector
{
    public class PerfectPostureDetector : PostureDetector, IDetector
    {
        public event Action<string> Detected;
        private const int AccumulatorTarget = 10;
        private readonly IEnumerable<IPosture> postures;

        public PerfectPostureDetector() : base(AccumulatorTarget)
        {
            PostureDetected += TriggerEvent;
            postures = new List<IPosture>
                {
                    new HandsJoinedPosture(), 
                    new HandsOverHeadPosture(), 
                    new HelloPosture()
                };
        }

        public override void TrackPostures(Skeleton skeleton)
        {
            Track(skeleton);
        }

        public void Track(Skeleton skeleton)
        {
            if (skeleton.TrackingState != SkeletonTrackingState.Tracked)
            {
                return;
            }

            var skeletonVector = new SkeletonVector(skeleton.Joints);
            foreach (var posture in postures.Where(posture => posture.IsDetected(skeletonVector)))
            {
                RaisePostureDetected(posture.Name);
                return;
            }

            Reset();
        }

        private void TriggerEvent(string eventName)
        {
            if (Detected != null && !string.IsNullOrEmpty(eventName))
            {
                Detected(eventName);
            }
        }
    }
}

