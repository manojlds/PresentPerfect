using System.IO;
using System.Linq;
using System.Windows.Controls;

using Microsoft.Kinect;

using PresentPerfect.Renderers;

namespace PresentPerfect
{
    public class SensorManager
    {
        private KinectSensor sensor;

        public void Start(Image image, TextBlock statusBar)
        {
            sensor = FindSensor();
            try
            {
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