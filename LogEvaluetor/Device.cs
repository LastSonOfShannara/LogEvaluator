using System;
using System.Collections.Generic;

namespace LogEvaluetor
{
    abstract class Device
    {
        protected const string outputFormat = "\"{0}\" : \"{1}\"";
        protected string name;
        protected List<(DateTime time, double value)> readings;
        protected double referenceValue;
        public bool IsEvelueted { get; set; }
        public Device(string name, double referenceValue)
        {
            this.name = name;
            this.referenceValue = referenceValue;

            readings = new List<(DateTime time, double value)>();
            IsEvelueted = false;
        }

        public void AddReading(string[] readingValues)
        {
            DateTime time;
            double value;

            if ((readingValues.Length != 2) || !DateTime.TryParse(readingValues[0], out time) || !double.TryParse(readingValues[1].Replace('.', ','), out value))
                throw new Exception($"Error processing reading of {name}: {readingValues}");

            readings.Add((time, value));
        }

        public abstract string Evaluate();

    }
}
