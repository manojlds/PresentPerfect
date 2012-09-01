using System;
using Microsoft.Kinect;
using PresentPerfect.Detector;

namespace PresentPerfect.Monitor
{
    public class SkeletonStreamMonitor
    {
        private readonly KinectSensor sensor;
        private readonly PerfectPostureDetector perfectPostureDetector;

        public SkeletonStreamMonitor(KinectSensor sensor)
        {
            this.sensor = sensor;
            perfectPostureDetector = new PerfectPostureDetector();
        }

        public void Start()
        {
            sensor.SkeletonStream.Enable();
            sensor.SkeletonFrameReady += SensorOnSkeletonFrameReady;
            perfectPostureDetector.PostureDetected += PerfectPostureDetectorOnPostureDetected;
        }

        private static void PerfectPostureDetectorOnPostureDetected(string posture)
        {
            Console.WriteLine("{0} | {1}", DateTime.Now, posture);
        }

        private void SensorOnSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs skeletonFrameReadyEventArgs)
        {
            var skeletons = RetrieveSkeletons(skeletonFrameReadyEventArgs);
            if (skeletons.Length == 0)
            {
                return;
            }

            foreach (var skeleton in skeletons)
            {
                perfectPostureDetector.TrackPostures(skeleton);
            }
        }

        private static Skeleton[] RetrieveSkeletons(SkeletonFrameReadyEventArgs skeletonFrameReadyEventArgs)
        {
            var skeletons = new Skeleton[0];
            using (var skeletonFrame = skeletonFrameReadyEventArgs.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletons);
                }
            }

            return skeletons;
        }
    }
}