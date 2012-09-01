using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Kinect;
using PresentPerfect.Gesture;

namespace PresentPerfect.Detector
{
    public class PerfectGestureDetector : IDetector
    {
        public event Action<string> Detected;
        private readonly IEnumerable<IGesture> gestures;

        public PerfectGestureDetector()
        {
            gestures = new List<IGesture>
                {
                    new SwipeGesture()
                };
        }

        public void Track(Skeleton skeleton)
        {
            if (skeleton.TrackingState != SkeletonTrackingState.Tracked)
                return;

            foreach (var posture in gestures.Where(gesture => gesture.IsDetected(skeleton)))
            {
                Detected(posture.Name);
                return;
            }
        }
    }
}

