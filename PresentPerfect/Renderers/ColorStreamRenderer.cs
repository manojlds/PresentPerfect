using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Kinect.Toolbox.Record;
using PresentPerfect.Source;

namespace PresentPerfect.Renderers
{
    public class ColorStreamRenderer
    {
        private readonly KinectSource kinectSource;
        public readonly WriteableBitmap ColorBitmap;
        private readonly byte[] colorPixelsFromCamera;

        public ColorStreamRenderer(KinectSource kinectSource)
        {
            this.kinectSource = kinectSource;
            colorPixelsFromCamera = new byte[kinectSource.FramePixelDataLength];
            ColorBitmap = new WriteableBitmap(kinectSource.FrameWidth, kinectSource.FrameHeight, 96.0, 96.0, PixelFormats.Bgr32, null);
        }

        public void Start(Image image)
        {
            image.Source = ColorBitmap;
            kinectSource.ColorFrameReady += SensorColorFrameReady;
        }

        private void SensorColorFrameReady(object sender, ReplayColorImageFrame colorFrame)
        {
            if (colorFrame == null)
            {
                return;
            }

            colorFrame.CopyPixelDataTo(colorPixelsFromCamera);
            ColorBitmap.WritePixels(
                new Int32Rect(0, 0, ColorBitmap.PixelWidth, ColorBitmap.PixelHeight),
                colorPixelsFromCamera,
                ColorBitmap.PixelWidth * sizeof(int),
                0);
        }
    }
}