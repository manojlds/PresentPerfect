using System.Windows;

namespace PresentPerfect
{
    public partial class PresentPerfectWindow : Window
    {
        private readonly SensorManager sensorManager;

        public PresentPerfectWindow()
        {
            InitializeComponent();
            sensorManager = new SensorManager();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            sensorManager.Start(Image, statusBarText, kinectCanvas);
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            sensorManager.End();
        }
    }
}
