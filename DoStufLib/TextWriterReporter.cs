using System.IO;

namespace DoStufLib
{
    /// <summary>
    ///     IStatusReporter that outputs to a TextWriter
    /// </summary>
    public class TextWriterReporter : IStatusReporter
    {
        private readonly TextWriter _writer;

        public TextWriterReporter(TextWriter writer)
        {
            _writer = writer;
        }

        public void ReportMessage(string message)
        {
            _writer.WriteLine(message);
        }

        public void ReportWarning(string warning)
        {
            _writer.WriteLine($"WARNING: {warning}");
        }

        public void ReportError(string error)
        {
            _writer.WriteLine($"ERROR: {error}");
        }
    }
}