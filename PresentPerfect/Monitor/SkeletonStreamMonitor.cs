using System;
using System.Windows.Controls;

using Kinect.Toolbox;

using Microsoft.Kinect;
using PresentPerfect.Detector;

namespace PresentPerfect.Monitor
{
    public class SkeletonStreamMonitor
    {
        private readonly KinectSensor sensor;
        private readonly PerfectPostureDetector perfectPostureDetector;
        private readonly SwipeGestureDetector swipeGestureDetector;

        private SkeletonDisplayManager skeletonDisplayManager;

        public SkeletonStreamMonitor(KinectSensor sensor)
        {
            this.sensor = sensor;
            perfectPostureDetector = new PerfectPostureDetector();
            swipeGestureDetector = new SwipeGestureDetector();
        }

        public void Start(Canvas kinectCanvas)
        {
            skeletonDisplayManager = new SkeletonDisplayManager(sensor, kinectCanvas);
            sensor.SkeletonStream.Enable(new TransformSmoothParameters
            {
                Smoothing = 0.5f,
                Correction = 0.5f,
                Prediction = 0.5f,
                JitterRadius = 0.05f,
                MaxDeviationRadius = 0.04f
            });
            sensor.SkeletonFrameReady += SensorOnSkeletonFrameReady;
            perfectPostureDetector.PostureDetected += PerfectPostureDetectorOnPostureDetected;
            swipeGestureDetector.OnGestureDetected += SwipeGestureDetectorOnOnGestureDetected;
        }

        private void SwipeGestureDetectorOnOnGestureDetected(string gesture)
        {
            Console.WriteLine("{0} | {1}", DateTime.Now, gesture);
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

            skeletonDisplayManager.Draw(skeletons, false);
            foreach (var skeleton in skeletons)
            {
                perfectPostureDetector.TrackPostures(skeleton);
               
                foreach (Joint joint in skeleton.Joints)
                {
                    if (joint.TrackingState != JointTrackingState.Tracked)
                        continue;

                    if (joint.JointType == JointType.HandLeft)
                    {
                        swipeGestureDetector.Add(joint.Position, sensor);
                    }
                }
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