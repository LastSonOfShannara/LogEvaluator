using System;
using System.Linq;

namespace LogEvaluetor
{
    class Thermometer : Device
    {
        public Thermometer(string name, double referenceValue) : base(name, referenceValue)
        {

        }

        public override string Evaluate()
        {
            IsEvelueted = true;
            double deviation = 0;
            double average_temp = readings.Sum((reading) => reading.value) / (double)readings.Count;
            foreach (var reading in readings)
            {
                var actualDeviation = Math.Abs(referenceValue - reading.value);
                if (actualDeviation > deviation)
                    deviation = actualDeviation;
                if (actualDeviation > 5)
                    return string.Format(outputFormat, name, "precise");
            }
            bool IsWithInRange = Evaluator.IsWithIn(average_temp, referenceValue - 0.5, referenceValue + 0.5);
            if ((deviation < 3) && IsWithInRange)
                return string.Format(outputFormat, name, "ultra precise");
            else if ((deviation < 5) && IsWithInRange)
                return string.Format(outputFormat, name, "very precise");
            else
                return string.Format(outputFormat, name, "precise");
        }
    }

    class Humidity : Device
    {
        public Humidity(string name, double referenceValue) : base(name, referenceValue)
        {

        }

        public override string Evaluate()
        {
            IsEvelueted = true;
            return KeepDiscardEvaluation(1);

        }
    }

    class Monoxide : Device
    {
        public Monoxide(string name, double referenceValue) : base(name, referenceValue)
        {

        }

        public override string Evaluate()
        {
            IsEvelueted = true;
            return KeepDiscardEvaluation(3);
        }
    }
}
