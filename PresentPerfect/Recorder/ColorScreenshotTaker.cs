using System;
using System.Globalization;
using System.IO;
using System.Windows.Media.Imaging;

namespace PresentPerfect.Recorder
{
    class ColorScreenshotTaker
    {
        public bool Capture(WriteableBitmap colorBitmap, DateTime detectionTime, string posture, out string filePath)
        {
            filePath = null;
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(colorBitmap));
            var path = GetScreenshotPath(detectionTime, posture);
            try
            {
                using (var fs = new FileStream(path, FileMode.Create))
                {
                    encoder.Save(fs);
                    filePath = path;
                    return true;
                }

            }
            catch (IOException)
            {
                return false;
            }
        }

        private static string GetScreenshotPath(DateTime detectionTime, string posture)
        {
            var time = detectionTime.ToString("hh'-'mm'-'ss", CultureInfo.CurrentUICulture.DateTimeFormat);

            var myPhotos = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            var path = Path.Combine(myPhotos, string.Format("PresentPerfect-{0}-{1}.png", posture, time));
            return path;
        }
    }
}
