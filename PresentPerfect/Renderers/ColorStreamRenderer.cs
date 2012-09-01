using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;

namespace PresentPerfect.Renderers
{
    public class ColorStreamRenderer
    {
        private readonly KinectSensor kinectSensor;
        private WriteableBitmap colorBitmap;
        private byte[] colorPixelsFromCamera;
        
        public ColorStreamRenderer(KinectSensor kinectSensor)
        {
            this.kinectSensor = kinectSensor;
        }

        public void StartRendering(System.Windows.Controls.Image image)
        {
            if (kinectSensor == null)
            {
                throw new IOException("No sensor detected");
            }

            kinectSensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
            colorPixelsFromCamera = new byte[kinectSensor.ColorStream.FramePixelDataLength];
            colorBitmap = new WriteableBitmap(
                kinectSensor.ColorStream.FrameWidth,
                kinectSensor.ColorStream.FrameHeight,
                96.0,
                96.0,
                PixelFormats.Bgr32,
                null);

            image.Source = colorBitmap;
            kinectSensor.ColorFrameReady += SensorColorFrameReady;
            kinectSensor.Start();
        }

        private void SensorColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (var colorFrame = e.OpenColorImageFrame())
            {
                if (colorFrame == null)
                {
                    return;
                }

                colorFrame.CopyPixelDataTo(colorPixelsFromCamera);
                colorBitmap.WritePixels(
                    new Int32Rect(0, 0, colorBitmap.PixelWidth, colorBitmap.PixelHeight),
                    colorPixelsFromCamera,
                    colorBitmap.PixelWidth * sizeof(int),
                    0);
            }
        }
    }
}