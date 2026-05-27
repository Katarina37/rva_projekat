using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirQuality.Component1.Services
{
    public class LogService
    {
        private readonly string logFilePath;

        public LogService()
        {
            logFilePath = "activity_log.txt";
        }

        public void Log(string message)
        {
            string entry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {message}";
            File.AppendAllText(logFilePath, entry + Environment.NewLine);
        }
    }
}
