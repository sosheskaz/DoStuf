namespace DoStufLib.Model
{
    public interface IReportable
    {
        IStatusReporter StatusReporter { get; set; }
    }
}