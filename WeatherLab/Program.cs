using System;
using System.Linq;


namespace WeatherLab
{
    class Program
    {
        static string dbfile = @".\data\climate.db";

        static void Main(string[] args)
        {
            var measurements = new WeatherSqliteContext(dbfile).Weather;

            var total_2020_precipitation = measurements
                .Where(filedata => filedata.year == 2020)
                .Select(filedata => filedata.precipitation);
            Console.WriteLine($"Total precipitation in 2020: {total_2020_precipitation.Sum()} mm\n");

            //
            // Heating Degree days have a mean temp of < 18C
            //   see: https://en.wikipedia.org/wiki/Heating_degree_day
            //

            // ?? TODO ??

            //
            // Cooling degree days have a mean temp of >=18C
            //

            // ?? TODO ??

            //
            // Most Variable days are the days with the biggest temperature
            // range. That is, the largest difference between the maximum and
            // minimum temperature
            //
            // Oh: and number formatting to zero pad.
            // 
            // For example, if you want:
            //      var x = 2;
            // To display as "0002" then:
            //      $"{x:d4}"
            //
            Console.WriteLine("Year\tHDD\tCDD");
            // ?? TODO ??
            var yearsData = measurements
                .GroupBy(filedata => filedata.year)
                .Select(filedata => new
                {
                    year = filedata.Key,
                    hdd = filedata.Where(x => x.meantemp < 18).Count(),
                    cdd = filedata.Where(x => x.meantemp >= 18).Count(),
                });

            foreach (var year in yearsData)
            {
                Console.WriteLine($"{year.year}\t{year.hdd}\t{year.cdd}");
            }

            Console.WriteLine("\nTop 5 Most Variable Days");
            Console.WriteLine("YYYY-MM-DD\tDelta");

            // ?? TODO ??
            var mostVariableDaysData = measurements
                .Select(data => new
                    {
                        yearDate = $"{data.year}-{data.month:d2}-{data.day:d2}",
                        yearDelta = (data.maxtemp - data.mintemp)
                    })
                .OrderByDescending(data => data.yearDelta);

            int sum = 0;
            foreach (var variableDay in mostVariableDaysData)
            {
                if (sum < 5)
                {
                    Console.WriteLine($"{variableDay.yearDate}\t{variableDay.yearDelta}");
                    sum++;
                } 
                else
                    break;
                
            }
        }
    }
}
