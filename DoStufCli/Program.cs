using System;
using System.Linq;
using DoStufLib;
using DoStufLib.Model;

namespace DoStufCli
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                return;
            }

            var cmdTask = ActivityTaskList.Instance[args[0]]?.GetConstructor(new Type[0])?.Invoke(new object[0]) as IActivityTaskInterface;
            if (cmdTask != null) cmdTask.StatusReporter = new ConsoleReporter();

            cmdTask?.SetArgsFromDictionary(ParseArgsToDictionary.Parse(args.Skip(1)));
            cmdTask?.Execute();
        }
    }
}