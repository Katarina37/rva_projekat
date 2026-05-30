using AirQuality.Component2.Models;
using System.Globalization;
using System.IO;
using System.Text;

namespace AirQuality.Component2.Services
{
    public class CsvExportService
    {
        public void Export(string path, StatisticsResult result)
        {
            var csv = new StringBuilder();
            csv.AppendLine("Metoda,Opis,Vrednost,Jedinica");
            csv.AppendLine(string.Format(CultureInfo.InvariantCulture,
                "{0},{1},{2},{3}",
                Escape(result.MethodName),
                Escape(result.Description),
                result.Value,
                Escape(result.Unit)));

            File.WriteAllText(path, csv.ToString(), Encoding.UTF8);
        }

        private string Escape(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
            {
                return "\"" + value.Replace("\"", "\"\"") + "\"";
            }

            return value;
        }
    }
}
