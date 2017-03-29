using System.Collections.Generic;

namespace DoStufLib.Model
{
    public interface IActivityTaskInterface : IReportable, IExecutableTask
    {
        void SetArgsFromDictionary(Dictionary<string, object> argPopulator);
    }
}