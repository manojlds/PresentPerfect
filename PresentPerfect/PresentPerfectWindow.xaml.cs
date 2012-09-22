using System.IO;
using System.Windows;
using System.Windows.Controls;
using Kinect.Toolbox.Record;
using MahApps.Metro.Controls;
using Microsoft.Win32;

using PresentPerfect.Recorder;

namespace PresentPerfect
{
    public partial class PresentPerfectWindow : MetroWindow
    {
        private readonly SensorManager sensorManager;
        private readonly SensorRecorder sensorRecorder;

        public PresentPerfectWindow()
        {
            InitializeComponent();
            sensorRecorder = new SensorRecorder();
            sensorManager = new SensorManager(sensorRecorder);
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            sensorManager.Start(Image, statusBarText);
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            sensorManager.End();
            sensorRecorder.End();
        }

        private void RecordOptionClick(object sender, RoutedEventArgs e)
        {
            recordOption.Content = sensorRecorder.Trigger();
        }
    }
}
