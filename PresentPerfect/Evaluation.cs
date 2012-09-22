using System;
using System.Collections.Generic;

namespace PresentPerfect
{
    public class Evaluation
    {
        private readonly List<DateTime> occurences = new List<DateTime>();
        public string Name { get; set; }
        public List<DateTime> Occurences
        {
            get { return occurences; }
        }

        public string ImageBase64String { get; set; }

        public void Occured()
        {
            Occurences.Add(DateTime.Now);
        }
    }
}