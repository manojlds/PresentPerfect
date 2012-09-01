using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Kinect.Toolbox.Record;

using Microsoft.Kinect;

using PresentPerfect.Source;

namespace PresentPerfect.Renderers
{
    public class ColorStreamRenderer
    {
        private readonly KinectSource kinectSource;
        private readonly WriteableBitmap colorBitmap;
        private readonly byte[] colorPixelsFromCamera;

        public ColorStreamRenderer(KinectSource kinectSource)
        {
            this.kinectSource = kinectSource;
            colorPixelsFromCamera = new byte[kinectSource.FramePixelDataLength];
            colorBitmap = new WriteableBitmap(kinectSource.FrameWidth, kinectSource.FrameHeight, 96.0, 96.0, PixelFormats.Bgr32, null);
        }

        public void Start(Image image)
        {
            image.Source = colorBitmap;
            kinectSource.ColorFrameReady += SensorColorFrameReady;
        }

        private void SensorColorFrameReady(object sender, ReplayColorImageFrame colorFrame)
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