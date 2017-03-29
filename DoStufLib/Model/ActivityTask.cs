using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace DoStufLib.Model
{
    [DataContract]
    public abstract class ActivityTask<TArgsType, TDerivedType> : IActivityTaskInterface
    {
        /// <summary>
        ///     Arguments provided to the task.
        /// </summary>
        public TArgsType Args { get; set; }

        /// <summary>
        ///     Reporter used by the task to execute.
        /// </summary>
        public IStatusReporter StatusReporter { get; set; }

        /// <summary>
        ///     Run the ActivityTask.
        /// </summary>
        public abstract bool Execute();

        /// <summary>
        ///     Uses a dictionary to dynamically assign values to argument names.
        /// </summary>
        /// <param name="argPopulator"> Dictionary of PropertyName/ValueToSet.</param>
        public void SetArgsFromDictionary(Dictionary<string, object> argPopulator)
        {
            if (Args == null)
            {
                Args = (TArgsType) typeof (TArgsType).GetConstructor(new Type[0])?.Invoke(new object[0]);
            }
            var argsDictionaryLower = argPopulator.ToDictionary(keyValue => keyValue.Key.ToLower(), keyValue => keyValue.Value);

            foreach (var propertyInfo in typeof (TArgsType).GetProperties())
            {
                var propertyNameLower = propertyInfo.Name.ToLower();
                if (argsDictionaryLower.ContainsKey(propertyNameLower))
                {
                    propertyInfo.SetValue(Args, argsDictionaryLower[propertyNameLower]);
                }
            }
        }

        /// <summary>
        ///     Creates a new DerivedType.
        /// </summary>
        /// <returns></returns>
        public static TDerivedType Construct()
        {
            return (TDerivedType) typeof (TDerivedType).GetConstructor(new Type[0])?.Invoke(new object[0]);
        }
    }
}