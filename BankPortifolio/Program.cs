using System;
using System.IO;
using System.Linq;
using System.Text;

namespace BankPortifolio
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ITrade trade;
            var path = @"C:\Temp\SampleInput.txt";            
            string[] lines = File.ReadAllLines(path, Encoding.UTF8);
            
            DateTime refDate = DateTime.ParseExact(lines[0], "mm/dd/yyyy", null);
            string[] splitLine;

            for (int i = 2; i < lines.Count(); i++)
            {
                splitLine = lines[i].Split(' ');

                trade = new Trade
                {
                    Value = Convert.ToDouble(splitLine[0]),
                    ClientSector = splitLine[1],
                    NextPaymetDate = DateTime.ParseExact(splitLine[2], "mm/dd/yyyy", null)
            };

                Console.WriteLine(trade.GetCategory(refDate));
            }

            Console.ReadKey();
        }
    }

    public interface ITrade
    {
        double Value { get; }
        string ClientSector { get; }
        DateTime NextPaymetDate { get; }
        string GetCategory(DateTime referenceDate);
    }

    public class Trade : ITrade
    {
        private const string NOTCATEGORIZED = "NOTCATEGORIZED";
        private const string EXPIRED = "EXPIRED";
        private const string HIGHRISK = "HIGHRISK";
        private const string MEDIUMRISK = "MEDIUMRISK";

        public double Value { get; set; }
        public string ClientSector { get; set; }
        public DateTime NextPaymetDate { get; set; }

        public string GetCategory(DateTime referenceDate)
        {
            if (this.NextPaymetDate.Subtract(referenceDate).TotalDays < 30)
                return EXPIRED;

            if (this.ClientSector.ToUpper() == "PRIVATE" && this.Value > 1000000)
                return HIGHRISK;

            else if (this.ClientSector.ToUpper() == "PUBLIC" && this.Value > 1000000)
                return MEDIUMRISK;

            else
                return NOTCATEGORIZED;
            
        }
    }
}
