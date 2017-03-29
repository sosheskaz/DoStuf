using System;
using System.IO;
using System.Linq;
using DoStufLib;

namespace DoStufJsonReader
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine();
            }
            foreach (var jsonFile in args)
            {
                if (!File.Exists(jsonFile))
                {
                    Console.WriteLine($"{jsonFile} does not exist. Terminating program.");
                    break;
                }
            }
            var activities = args.Select(jsonFile => JsonToActivity.ConvertJson(File.ReadAllText(jsonFile)));

            foreach (var activity in activities)
            {
                activity.Reporter = new ConsoleReporter();
                activity.Execute();
            }

            Console.ReadLine();
        }
    }
}