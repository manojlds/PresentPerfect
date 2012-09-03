﻿using System.IO;
using System.Linq;
using System.Windows.Controls;
using Kinect.Toolbox.Record;
using Microsoft.Kinect;
using Microsoft.Win32;
using PresentPerfect.Monitor;
using PresentPerfect.Recorder;
using PresentPerfect.Renderers;
using PresentPerfect.Source;

namespace PresentPerfect
{
    public class SensorManager
    {
        private readonly SensorRecorder sensorRecorder;
        private KinectSensor kinectSensor;

        public SensorManager(SensorRecorder sensorRecorder)
        {
            this.sensorRecorder = sensorRecorder;
        }

        public void Start(Image image, TextBlock statusBar)
        {
            var kinectSource = DetermineKinectSource();
            try
            {
                var colorStreamRenderer = new ColorStreamRenderer(kinectSource);
                new SkeletonStreamMonitor(kinectSource, colorStreamRenderer).Start();
                colorStreamRenderer.Start(image);
            }
            catch (IOException)
            {
                statusBar.Text = "No Kinect source found";
            }
        }

        private KinectSource DetermineKinectSource()
        {
            kinectSensor = FindSensor();
            if (kinectSensor != null)
            {
                EnableSkeletonStream(kinectSensor);
                EnableColourStream(kinectSensor);
                kinectSensor.Start();
                return new KinectSource(kinectSensor, sensorRecorder);
            }

            return RetrieveFromFileSource();
        }

        private static KinectSource RetrieveFromFileSource()
        {
            var openFileDialog = new OpenFileDialog { Title = "Select filename", Filter = "Replay files|*.replay" };
            if (openFileDialog.ShowDialog() == true)
            {
                Stream recordStream = File.OpenRead(openFileDialog.FileName);
                var replay = new KinectReplay(recordStream);
                replay.Start();
                return new KinectSource(replay);
            }

            return null;
        }

        private static void EnableColourStream(KinectSensor sensor)
        {
            sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
        }

        private static void EnableSkeletonStream(KinectSensor sensor)
        {
            sensor.SkeletonStream.Enable(new TransformSmoothParameters {
                Smoothing = 0.5f, 
                Correction = 0.5f, 
                Prediction = 0.5f, 
                JitterRadius = 0.05f, 
                MaxDeviationRadius = 0.04f
            });
        }

        public void End()
        {
            if (kinectSensor != null)
            {
                kinectSensor.Stop();
            }
        }

        private static KinectSensor FindSensor()
        {
            return KinectSensor.KinectSensors.FirstOrDefault(potentialSensor => potentialSensor.Status == KinectStatus.Connected);
        }
    }
}