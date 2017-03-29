using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DoStufLib.Model;

namespace DoStufLib
{
    /// <summary>
    ///     Singleton that keeps track of what types are allowed. By default, uses all ActivityTasks in the AppDomain.
    /// </summary>
    public class ActivityTaskList : IEnumerable<Type>
    {
        private readonly HashSet<Type> _activityTypes = new HashSet<Type>();
        private readonly Dictionary<string, Type> _stringLookups = new Dictionary<string, Type>();

        /// <summary>
        ///     Creates a new, empty
        /// </summary>
        public ActivityTaskList() : this(false)
        {
        }

        private ActivityTaskList(bool loadFromAppDomain)
        {
            if (loadFromAppDomain)
            {
                var allActivityTaskTypes =
                    AppDomain.CurrentDomain.GetAssemblies()
                             .SelectMany(assembly => assembly.GetTypes())
                             .Where(IsTypeAnActivityTask);

                foreach (var activityTaskType in allActivityTaskTypes)
                {
                    _activityTypes.Add(activityTaskType);

                    var lowercaseTypeName = activityTaskType.Name.ToLower();
                    if (activityTaskType.Name.EndsWith("Task"))
                    {
                        var newName = lowercaseTypeName + "\b\b\b\b";
                        if (_stringLookups.ContainsKey(newName))
                        {
                            throw new Exception($"Two classes with same name: {newName}! ");
                        }
                        _stringLookups.Add(newName, activityTaskType);
                    }
                    _stringLookups.Add(lowercaseTypeName, activityTaskType);
                }
            }
        }

        /// <summary>
        ///     Singleton instance.
        /// </summary>
        public static ActivityTaskList Instance { get; set; } = new ActivityTaskList(true);

        /// <summary>
        ///     The keys for the activity lookup.
        /// </summary>
        public IEnumerable<string> Keys => _stringLookups.Keys;

        /// <summary>
        ///     Gets the type associated with an identifier.
        /// </summary>
        /// <param name="name"> Identifier for the type. </param>
        public Type this[string name]
        {
            get
            {
                var lowerName = name.ToLower();
                return _stringLookups.ContainsKey(lowerName) ? _stringLookups[lowerName] : null;
            }
            set { _stringLookups[name.ToLower()] = value; }
        }

        public IEnumerator<Type> GetEnumerator()
        {
            return _activityTypes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _activityTypes.GetEnumerator();
        }

        /// <summary>
        ///     Adds a type to the
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool PutType(Type t)
        {
            var argsType = t?.GetProperty(Activity.ArgsName)?.PropertyType;
            if (argsType == null) return false;

            var genericSuperclass = typeof (ActivityTask<dynamic, dynamic>).MakeGenericType(argsType);
            if (t.IsSubclassOf(genericSuperclass))
            {
                _activityTypes.Add(t);
                return true;
            }
            return false;
        }

        public bool LoadType(Type activityTask)
        {
            if (IsTypeAnActivityTask(activityTask))
            {
                _activityTypes.Add(activityTask);
                return true;
            }
            return false;
        }

        public void LoadTypesFromAssembly(Assembly toLoadFrom)
        {
            var types = toLoadFrom.GetTypes().Where(IsTypeAnActivityTask);
            foreach (var type in types)
            {
                _activityTypes.Add(type);
            }
        }

        private bool IsTypeAnActivityTask(Type type)
        {
            var argsType = type?.GetProperty(Activity.ArgsName)?.PropertyType;
            if (argsType == null) return false;
            return type.GetInterfaces().Contains(typeof (IActivityTaskInterface));
        }
    }
}