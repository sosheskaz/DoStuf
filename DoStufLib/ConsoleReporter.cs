using System;

namespace DoStufLib
{
    /// <summary>
    ///     IStatusReporter that reports to the console, in color.
    /// </summary>
    public class ConsoleReporter : IStatusReporter
    {
        public void ReportMessage(string message)
        {
            Console.ResetColor();
            Console.WriteLine(message);
        }

        public void ReportWarning(string warning)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"WARNING: {warning}");
            Console.ResetColor();
        }

        public void ReportError(string error)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"ERROR: {error}");
            Console.ResetColor();
        }
    }
}