using System;
using System.Linq;
using DoStufLib.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DoStufLib
{
    public static class JsonToActivity
    {
        public static Activity ConvertJson(string json)
        {
            var activity = new Activity();

            dynamic jsonObject = JsonConvert.DeserializeObject(json);
            dynamic activityTaskDynamics = jsonObject.ActivityTasks;
            var activityTasks = new IActivityTaskInterface[((JArray) activityTaskDynamics)?.Count ?? 0];

            if (activityTaskDynamics == null)
                return null;

            var index = 0;
            foreach (var activityTaskDynamic in activityTaskDynamics as JArray)
            {
                if ((activityTaskDynamic as dynamic).Type == null)
                {
                    Console.WriteLine("No type was provided.");
                }

                var properties = ((JToken) ((dynamic) activityTaskDynamic).Args).Children<JProperty>();
                var argsAsStrings = properties.SelectMany(property =>
                                                          {
                                                              try
                                                              {
                                                                  return new[] {property.Name, property.Value.ToString()};
                                                              }
                                                              catch (Exception)
                                                              {
                                                                  return new string[0];
                                                              }
                                                          }).ToArray(); // avoid multiple enumeration
                foreach (var argsAsString in argsAsStrings)
                {
                    Console.WriteLine("NEW ONE: " + argsAsString);
                }
                var argsDictionary = ParseArgsToDictionary.Parse(argsAsStrings);

                var activityTaskType = (Type) ActivityTaskList.Instance[((dynamic) activityTaskDynamic).Type.ToString()];

                var activityTask = activityTaskType?.GetConstructor(new Type[0])?.Invoke(new object[0]) as IActivityTaskInterface;
                if (activityTask == null) continue;

                foreach (var o in argsDictionary)
                {
                    Console.WriteLine($"ENTRY: {o.Key}: {o.Value}");
                }

                activityTask.SetArgsFromDictionary(argsDictionary);
                activity.Reporter = new ConsoleReporter();

                activityTasks[index] = activityTask;

                index++;
            }

            activity.Tasks = activityTasks;

            return activity;
        }
    }
}