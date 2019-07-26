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
                throw new ReadingBadFormatException($"Error processing reading of {name}: { string.Join(' ', readingValues) }");

            readings.Add((time, value));
        }
        protected string KeepDiscardEvaluation(int limit)
        {
            foreach (var reading in readings)
            {
                var actualDeviation = Math.Abs(referenceValue - reading.value);
                if (actualDeviation > limit)
                    return string.Format(outputFormat, name, "discard");
            }
            return string.Format(outputFormat, name, "keep");
        }

        public abstract string Evaluate();

    }
}
