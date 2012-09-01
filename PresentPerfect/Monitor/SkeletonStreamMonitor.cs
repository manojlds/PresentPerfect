using System;
using Kinect.Toolbox.Record;
using PresentPerfect.Detector;
using PresentPerfect.Source;

namespace PresentPerfect.Monitor
{
    public class SkeletonStreamMonitor
    {
        private readonly KinectSource kinectSource;
        private readonly PerfectPostureDetector perfectPostureDetector;

        public SkeletonStreamMonitor(KinectSource kinectSource)
        {
            this.kinectSource = kinectSource;
            perfectPostureDetector = new PerfectPostureDetector();
        }

        public void Start()
        {
            kinectSource.SkeletonFrameReady += KinectSourceOnSkeletonFrameReady;
            perfectPostureDetector.PostureDetected += PerfectPostureDetectorOnPostureDetected;
        }

        private static void PerfectPostureDetectorOnPostureDetected(string posture)
        {
            Console.WriteLine("{0} | {1}", DateTime.Now, posture);
        }

        private void KinectSourceOnSkeletonFrameReady(object sender, ReplaySkeletonFrame replaySkeletonFrame)
        {
            var skeletons = replaySkeletonFrame.Skeletons;
            if (skeletons.Length == 0)
            {
                return;
            }

            foreach (var skeleton in skeletons)
            {
                perfectPostureDetector.TrackPostures(skeleton);
            }
        }
    }
}