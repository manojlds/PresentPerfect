using System;
using System.Collections.Generic;
using Kinect.Toolbox.Record;
using Microsoft.Kinect;
using PresentPerfect.Detector;
using PresentPerfect.Recorder;
using PresentPerfect.Source;

namespace PresentPerfect.Monitor
{
    public class SkeletonStreamMonitor
    {
        private readonly KinectSource kinectSource;

        private readonly IList<IDetector> detectors;

        public SkeletonStreamMonitor(KinectSource kinectSource)
        {
            this.kinectSource = kinectSource;
            detectors = new List<IDetector>
                {
                    new PerfectPostureDetector(),
                    new PerfectGestureDetector()
                };
        }

        public void Start()
        {
            kinectSource.SkeletonFrameReady += KinectSourceOnSkeletonFrameReady;
            foreach (var detector in detectors)
            {
                detector.Detected += EventDetected;
            }
        }

        public event Action<ObservationEventArgs> OnObservation;

        private void EventDetected(IObservation observation)
        {
            OnObservation(new ObservationEventArgs { Observation = observation });
        }

        private void KinectSourceOnSkeletonFrameReady(object sender, ReplaySkeletonFrame replaySkeletonFrame)
        {
            var skeletons = replaySkeletonFrame.Skeletons;
            if (skeletons.Length == 0)
            {
                return;
            }

            PassSkeletonsToDetectors(skeletons);
        }

        private void PassSkeletonsToDetectors(IEnumerable<Skeleton> skeletons)
        {
            foreach (var skeleton in skeletons)
            {
                foreach (var detector in detectors)
                {
                    detector.Track(skeleton);
                }
            }
        }
    }
}