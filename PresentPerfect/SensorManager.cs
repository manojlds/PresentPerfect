using System.IO;
using System.Linq;
using System.Windows.Controls;

using Microsoft.Kinect;

using PresentPerfect.Monitor;
using PresentPerfect.Renderers;

namespace PresentPerfect
{
    public class SensorManager
    {
        private KinectSensor sensor;

        public void Start(Image image, TextBlock statusBar, Canvas kinectCanvas)
        {
            sensor = FindSensor();
            try
            {
                new SkeletonStreamMonitor(sensor).Start(kinectCanvas);
                new ColorStreamRenderer(sensor).StartRendering(image);
            }
            catch (IOException)
            {
                statusBar.Text = "No Kinect found";
            }
        }

        public void End()
        {
            if (sensor != null)
            {
                sensor.Stop();
            }
        }

        private static KinectSensor FindSensor()
        {
            return KinectSensor.KinectSensors.FirstOrDefault(potentialSensor => potentialSensor.Status == KinectStatus.Connected);
        }
    }
}