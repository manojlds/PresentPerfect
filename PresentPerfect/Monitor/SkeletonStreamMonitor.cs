using System;
using System.Collections.Generic;
using Kinect.Toolbox.Record;
using Microsoft.Kinect;
using PresentPerfect.Detector;
using PresentPerfect.Recorder;
using PresentPerfect.Renderers;
using PresentPerfect.Source;

namespace PresentPerfect.Monitor
{
    public class SkeletonStreamMonitor
    {
        private readonly KinectSource kinectSource;

        private readonly ColorStreamRenderer colorStreamRenderer;

        private readonly IList<IDetector> detectors;

        private readonly ColorScreenshotTaker colorScreenshotTaker;

        public SkeletonStreamMonitor(KinectSource kinectSource, ColorStreamRenderer colorStreamRenderer)
        {
            this.kinectSource = kinectSource;
            this.colorStreamRenderer = colorStreamRenderer;
            colorScreenshotTaker = new ColorScreenshotTaker();
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

        private void EventDetected(string posture)
        {
            var detectionTime = DateTime.Now;
            colorScreenshotTaker.Capture(colorStreamRenderer.ColorBitmap,detectionTime, posture);
            Console.WriteLine("{0} | {1}", detectionTime, posture);
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