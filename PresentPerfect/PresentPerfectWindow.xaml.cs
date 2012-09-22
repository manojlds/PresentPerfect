using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
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

        private readonly Dictionary<string, Evaluation> perfectEvaluation = new Dictionary<string, Evaluation>();

        public PresentPerfectWindow()
        {
            InitializeComponent();
            sensorRecorder = new SensorRecorder();
            sensorManager = new SensorManager(sensorRecorder);
            sensorManager.OnObservation += OnObservation;
        }

        private void StartClick(object sender, RoutedEventArgs e)
        {
            perfectEvaluation.Clear();
            sensorManager.Start(Image);
        }

        private void StopClick(object sender, RoutedEventArgs e)
        {
            SaveEvaluation();
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

            Evaluate(args);
        }

        private void Evaluate(ObservationEventArgs args)
        {
            Evaluation evaluation;
            var observationName = args.Observation.Name;
            perfectEvaluation.TryGetValue(observationName, out evaluation);
            evaluation = evaluation ?? new Evaluation { Name = observationName, ImageBase64String = ImageToBase64(args.Image) };
            evaluation.Occured();
            perfectEvaluation[observationName] = evaluation;

        }

        public string ImageToBase64(WriteableBitmap image)
        {
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            using (var ms = new MemoryStream())
            {
                encoder.Save(ms);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        private void SetDetailsPanel(ObservationEventArgs args)
        {
            snap.Source = args.Image.Clone();
            this.gestureText.Text = args.Observation.Name;
        }

        private void SaveEvaluation()
        {
            var time = DateTime.Now.ToString("hh'-'mm'-'ss", CultureInfo.CurrentUICulture.DateTimeFormat);
            var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), string.Format("PresentPerfect-{0}.ppe", time));
            var xmlSerializer = new XmlSerializer(typeof(List<Evaluation>));
            using (var fileStream = new FileStream(fileName, FileMode.Create))
            {
                xmlSerializer.Serialize(fileStream, perfectEvaluation.Values.ToList());
            }
        }
    }
}
