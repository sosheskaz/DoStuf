using System.Diagnostics;
using System.Runtime.Serialization;
using DoStufLib.Model;

namespace DoStufLib.ActivityTasks
{
    public class CommandTask : ActivityTask<CommandTask.CommandTaskArgs, CommandTask>
    {
        public override bool Execute()
        {
            StatusReporter?.ReportMessage($"Running \"{Args.Command}\" with args \"{Args.Args?.Trim()}\"");
            var process = Process.Start("CMD.exe", "/C " + Args.Command + " " + Args.Args);
            if (process == null)
                return false;
            process.WaitForExit();
            var exitCode = process.ExitCode;
            if (exitCode != 0)
            {
                StatusReporter?.ReportWarning($"Process exited with exit code {exitCode}.");
            }
            else
            {
                StatusReporter?.ReportMessage($"Process exited with exit code {exitCode}.");
            }
            return exitCode == 0;
        }

        [DataContract]
        public class CommandTaskArgs
        {
            [DataMember]
            public string Command { get; set; }

            [DataMember]
            public string Args { get; set; }
        }
    }
}