using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;
using PresentPerfect.Detector;
using PresentPerfect.Recorder;

namespace PresentPerfect
{
    public partial class PresentPerfectWindow : MetroWindow
    {
        private readonly SensorManager sensorManager;
        private readonly SensorRecorder sensorRecorder;
        private readonly SolidColorBrush green = new SolidColorBrush(Colors.Green);
        private readonly SolidColorBrush red = new SolidColorBrush(Colors.Red);

        private readonly IDictionary<string, Evaluation> perfectEvaluation = new Dictionary<string, Evaluation>();
        private ColorScreenshotTaker colorScreenshotTaker;

        public PresentPerfectWindow()
        {
            InitializeComponent();
            sensorRecorder = new SensorRecorder();
            sensorManager = new SensorManager(sensorRecorder);
            sensorManager.OnObservation += OnObservation;
            colorScreenshotTaker = new ColorScreenshotTaker();
        }

        private void StartClick(object sender, RoutedEventArgs e)
        {
            sensorManager.Start(Image);
        }

        private void StopClick(object sender, RoutedEventArgs e)
        {
            sensorManager.End();
            sensorRecorder.End();
        }

        private void RecordOptionClick(object sender, RoutedEventArgs e)
        {
            recordOption.Content = sensorRecorder.Trigger();
        }

        private void OnObservation(ObservationEventArgs args)
        {
            viewBoxBorder.BorderBrush = args.Observation.IsPositive ? green : red;
            SetDetailsPanel(args);
        }

        private void SetDetailsPanel(ObservationEventArgs args)
        {
            snap.Source = args.Image.Clone();
            this.gestureText.Text = args.Observation.Name;
        }
    }
}
