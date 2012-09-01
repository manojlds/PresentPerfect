using System;
using System.Collections.Generic;
using Kinect.Toolbox;
using Microsoft.Kinect;

namespace PresentPerfect.Gesture
{
    public abstract class Gesture : IGesture
    {
        public int MinimalPeriodBetweenGestures { get; set; }
        private readonly List<Entry> entries = new List<Entry>();
        private DateTime lastGestureDate = DateTime.Now;
        private readonly int entryWindowSize;

        protected Gesture(int entryWindowSize = 20)
        {
            this.entryWindowSize = entryWindowSize;
            MinimalPeriodBetweenGestures = 0;
        }

        public string Name { get; protected set; }

        protected List<Entry> Entries
        {
            get { return entries; }
        }

        public int EntryWindowSize
        {
            get { return entryWindowSize; }
        }

        public virtual void Add(SkeletonPoint position)
        {
            var newEntry = new Entry {Position = position.ToVector3(), Time = DateTime.Now};
            Entries.Add(newEntry);
            RemoveOldEntries();
        }

        private void RemoveOldEntries()
        {
            if (Entries.Count <= EntryWindowSize)
            {
                return;
            }

            var entryToRemove = Entries[0];
            Entries.Remove(entryToRemove);
        }

        protected void RaiseGestureDetected()
        {
            if (DateTime.Now.Subtract(lastGestureDate).TotalMilliseconds > MinimalPeriodBetweenGestures)
            {
                lastGestureDate = DateTime.Now;
            }

            Entries.Clear();
        }

        public abstract bool IsDetected(Skeleton skeleton);
    }
}