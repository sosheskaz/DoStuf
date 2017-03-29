using System.Collections.Generic;
using System.Linq;

namespace DoStufLib
{
    public static class ParseArgsToDictionary
    {
        public static Dictionary<string, object> Parse(IEnumerable<string> args)
        {
            var dict = new Dictionary<string, object>();

            var argsArray = args.ToArray();

            for (var i = 1; i < argsArray.Length; i += 2)
            {
                dict.Add(argsArray[i - 1], argsArray[i]);
            }

            return dict;
        }
    }
}