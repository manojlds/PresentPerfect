using System;
using Kinect.Toolbox.Record;
using Microsoft.Kinect;

using PresentPerfect.Recorder;

namespace PresentPerfect.Source
{
    public class KinectSource
    {
        private readonly SensorRecorder sensorRecorder;

        private const int DefaultPixelDataLength = 1228800;
        private const int DefaultFrameWidth = 640;
        private const int DefaultFrameHeight = 480;
        public event Action<object, ReplayColorImageFrame> ColorFrameReady;
        public event Action<object, ReplaySkeletonFrame> SkeletonFrameReady;
        public event Action<object, ReplayDepthImageFrame> DepthFrameReady;

        public KinectSource(KinectReplay kinectReplay)
        {
            kinectReplay.ColorImageFrameReady += ReplayColorFrameReady;
            kinectReplay.SkeletonFrameReady += ReplaySkeletonFrameReady;
            kinectReplay.DepthImageFrameReady += ReplayDepthFrameReady;
            FramePixelDataLength = DefaultPixelDataLength;
            FrameWidth = DefaultFrameWidth;
            FrameHeight = DefaultFrameHeight;
        }

        public KinectSource(KinectSensor kinectSensor, SensorRecorder sensorRecorder)
        {
            this.sensorRecorder = sensorRecorder;
            kinectSensor.ColorFrameReady += SensorColorFrameReady;
            kinectSensor.SkeletonFrameReady += SensorSkeletonFrameReady;
            kinectSensor.DepthFrameReady += SensorDepthFrameReady;
            FramePixelDataLength = kinectSensor.ColorStream.FramePixelDataLength;
            FrameWidth = kinectSensor.ColorStream.FrameWidth;
            FrameHeight = kinectSensor.ColorStream.FrameHeight;
        }

        public int FrameHeight { get; private set; }
        public int FrameWidth { get; private set; }
        public int FramePixelDataLength { get; private set; }

        private void SensorDepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        {
            if (DepthFrameReady == null)
            {
                return;
            }

            using (var frame = e.OpenDepthImageFrame())
            {
                if (frame != null)
                {
                    sensorRecorder.Record(frame);
                    DepthFrameReady(sender, frame);
                }
            }
        }

        private void ReplayDepthFrameReady(object sender, ReplayDepthImageFrameReadyEventArgs e)
        {
            if (DepthFrameReady != null)
            {
                DepthFrameReady(sender, e.DepthImageFrame);
            }
        }

        private void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            if (SkeletonFrameReady == null)
            {
                return;
            }

            using (var frame = e.OpenSkeletonFrame())
            {
                if (frame != null)
                {
                    sensorRecorder.Record(frame);
                    SkeletonFrameReady(sender, frame);
                }
            }
        }

        private void ReplaySkeletonFrameReady(object sender, ReplaySkeletonFrameReadyEventArgs e)
        {
            if (SkeletonFrameReady != null)
            {
                SkeletonFrameReady(sender, e.SkeletonFrame);
            }
        }

        private void SensorColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            if (ColorFrameReady == null)
            {
                return;
            }

            using (var frame = e.OpenColorImageFrame())
            {
                if (frame != null)
                {
                    sensorRecorder.Record(frame);
                    ColorFrameReady(sender, frame);
                }
            }
        }

        private void ReplayColorFrameReady(object sender, ReplayColorImageFrameReadyEventArgs e)
        {
            if (ColorFrameReady != null)
            {
                ColorFrameReady(sender, e.ColorImageFrame);
            }
        }
    }
}
