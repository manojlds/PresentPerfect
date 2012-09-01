using System.IO;
using Kinect.Toolbox.Record;
using Microsoft.Kinect;
using Microsoft.Win32;

namespace PresentPerfect.Recorder
{
    public class SensorRecorder
    {
        private KinectRecorder recorder;

        public string Trigger()
        {
            if (recorder != null)
            {
                End();
                return "Start Recording";
            }

            DetermineSaveRecordFile();
            return "Stop Recording";
        }

        private void DetermineSaveRecordFile()
        {
            var saveFileDialog = new SaveFileDialog { Title = "Select filename", Filter = "Replay files|*.replay" };
            if (saveFileDialog.ShowDialog() == true)
            {
                Start(saveFileDialog.FileName);
            }
        }

        public void Start(string targetFileName)
        {
            Stream recordStream = File.Create(targetFileName);
            recorder = new KinectRecorder(KinectRecordOptions.Skeletons | KinectRecordOptions.Color, recordStream);
        }

        public void Record(SkeletonFrame skeletonFrame)
        {
            if (recorder != null)
            {
                recorder.Record(skeletonFrame);
            }
        }

        public void Record(DepthImageFrame depthImageFrame)
        {
            if (recorder != null)
            {
                recorder.Record(depthImageFrame);
            }
        }

        public void Record(ColorImageFrame colorImageFrame)
        {
            if (recorder != null)
            {
                recorder.Record(colorImageFrame);
            }
        }

        public void End()
        {
            if (recorder == null)
            {
                return;
            }

            recorder.Stop();
            recorder = null;
        }
    }
}
