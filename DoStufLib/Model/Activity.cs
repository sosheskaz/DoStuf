using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace DoStufLib.Model
{
    /// <summary>
    ///     An activity to be run.
    /// </summary>
    [DataContract]
    public class Activity
    {
        public const string ArgsName = "Args";

        /// <summary>
        ///     Creates an Activity that reports to the Console.
        /// </summary>
        public Activity() : this(new ConsoleReporter())
        {
        }

        /// <summary>
        ///     Creates an activity.
        /// </summary>
        /// <param name="reporter"> Where the activity reports to. </param>
        public Activity(IStatusReporter reporter)
        {
            Reporter = reporter;
        }

        public IStatusReporter Reporter { get; set; }

        /// <summary>
        ///     The identifying name of the activity.
        /// </summary>
        [DataMember]
        public string Name { get; set; } = "Activity";

        /// <summary>
        ///     Tasks to be carried out by this activity.
        /// </summary>
        [DataMember]
        public IActivityTaskInterface[] Tasks { get; set; }

        /// <summary>
        ///     Executes all of the tasks sequentially, in order.
        /// </summary>
        public void Execute()
        {
            foreach (var activityTask in Tasks.Where(activityTask => activityTask != null))
            {
                activityTask.StatusReporter = Reporter;
                activityTask.Execute();
            }
        }

        public static IEnumerable<Type> GetActivities()
        {
            var activityClasses = Assembly.GetExecutingAssembly().GetTypes()
                                          .Where(type =>
                                                 {
                                                     var argsType = type.GetProperty(ArgsName)?.PropertyType;
                                                     if (argsType == null) return false;
                                                     var genericSuperclass = typeof (ActivityTask<dynamic, dynamic>).MakeGenericType(argsType, type);
                                                     return type.IsSubclassOf(genericSuperclass);
                                                 });

            activityClasses = activityClasses.Concat(
                Assembly.GetEntryAssembly().GetTypes()
                        .Where(type =>
                               {
                                   var argsType = type.GetProperty(ArgsName)?.PropertyType;
                                   if (argsType == null) return false;
                                   var genericSuperclass = typeof (ActivityTask<dynamic, dynamic>).MakeGenericType(argsType);
                                   return type.IsSubclassOf(genericSuperclass);
                               }));

            return activityClasses;
        }
    }
}