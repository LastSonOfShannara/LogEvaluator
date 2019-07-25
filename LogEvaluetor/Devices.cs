using System;

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
            foreach (var reading in readings)
            {
                var actualDeviation = Math.Abs(referenceValue - reading.value);
                if (actualDeviation > deviation)
                    deviation = actualDeviation;
                if (actualDeviation > 5)
                    return string.Format(outputFormat, name, "precise");
            }
            if (deviation >= 3)
                return string.Format(outputFormat, name, "very precise");
            else
                return string.Format(outputFormat, name, "ultra precise");
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
            foreach (var reading in readings)
            {
                var actualDeviation = Math.Abs(referenceValue - reading.value);
                if (actualDeviation > 1)
                    return string.Format(outputFormat, name, "discard");
            }
            return string.Format(outputFormat, name, "keep");

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
            foreach (var reading in readings)
            {
                var actualDeviation = Math.Abs(referenceValue - reading.value);
                if (actualDeviation > 3)
                    return string.Format(outputFormat, name, "discard");
            }
            return string.Format(outputFormat, name, "keep");
        }
    }
}
