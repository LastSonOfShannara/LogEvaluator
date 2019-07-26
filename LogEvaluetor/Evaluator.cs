using System;
using System.IO;
using System.Text;

namespace LogEvaluetor
{
    public static class Evaluator
    {
        private struct RefernceReading
        {
            public double tempreture;
            public double humidity;
            public double carb_monoxide;
        }

        public static bool IsDateTime(string val) => DateTime.TryParse(val, out DateTime result);
        public static bool IsWithIn<T>(T value, T min, T max) where T : IComparable 
        {
            return (value.CompareTo(min) >= 0) && (value.CompareTo(max) <= 0);
        }

        /// <summary>
        /// Evaluation of log of Devices and their readings
        /// </summary>
        /// <param name="logContentsStr">String represantation of log file</param>
        /// <returns>
        /// String representation of evaluated devices.
        /// </returns>
        /// <exception cref="LogEvaluetor.UnknownDeviceException"></exception>
        /// <exception cref="LogEvaluetor.ReferenceLineException"></exception>
        /// <exception cref="LogEvaluetor.DeviceNotInicializedException"></exception>
        /// <exception cref="LogEvaluetor.ReadingBadFormatException"></exception>



        public static string EvaluateLogFile(string logContentsStr)
        {
            if (string.IsNullOrEmpty(logContentsStr))
                return "";

            var sb = new StringBuilder();
            sb.AppendLine("{");

            using (var stream = new StringReader(logContentsStr))
            {
                var referenceReading = GetRefernceReading(stream.ReadLine().Split(' '));
                if (referenceReading == null)
                    throw new ReferenceLineException("Reference line is either missing or in incorrect format");

                Device device = null;
                string line;
                while ((line = stream.ReadLine()) != null)
                {
                    var reading = line.Split(' ');

                    if (IsDateTime(reading[0])) //Reading detected
                    {
                        if (device != null && !device.IsEvelueted)
                            device.AddReading(reading);
                        else
                            throw new DeviceNotInicializedException("Error handeling reading. Actual device is either not initialized or it is already evaluated");
                    }
                    else
                    {
                        if (device != null)
                            sb.Append(device.Evaluate()).AppendLine(",");
                        switch (reading[0])
                        {
                            case "thermometer":
                                device = new Thermometer(reading[1], referenceReading.Value.tempreture);
                                break;
                            case "humidity":
                                device = new Humidity(reading[1], referenceReading.Value.humidity);
                                break;
                            case "monoxide":
                                device = new Monoxide(reading[1], referenceReading.Value.carb_monoxide);
                                break;
                            default:
                                throw new UnknownDeviceException($"This utility doesn´t support this device: {reading[1]}");
                        }
                    }
                }
                if (device != null && !device.IsEvelueted)
                    sb.AppendLine(device.Evaluate());
            }

            sb.AppendLine("}");

            return sb.ToString();
        }

        private static RefernceReading? GetRefernceReading(string[] values)
        {
            RefernceReading refernceReading = new RefernceReading();
            if (!values[0].Equals("reference", StringComparison.CurrentCultureIgnoreCase))
                return null;
            if (!double.TryParse(values[1].Replace('.', ','), out refernceReading.tempreture))
                return null;
            if (!double.TryParse(values[2].Replace('.', ','), out refernceReading.humidity))
                return null;
            if (!double.TryParse(values[3].Replace('.', ','), out refernceReading.carb_monoxide))
                return null;
            return refernceReading;
        }
    }
}
