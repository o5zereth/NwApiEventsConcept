namespace NwApiEventsConcept.Events
{
    using NwApiEventsConcept.Core.Attributes;
    using NwApiEventsConcept.Core.Delegates;
    using NwApiEventsConcept.Core.Enums;
    using NwApiEventsConcept.Core.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class EventManager
    {
        private static readonly Dictionary<ServerEventType, FieldInfo> DelegateFields
            = new Dictionary<ServerEventType, FieldInfo>();

        private static readonly Dictionary<ServerEventType, bool> EventIsMultiReturn
            = new Dictionary<ServerEventType, bool>();

        internal static void Init()
        {
            foreach (var type in typeof(EventManager).Assembly.GetTypes())
            {
                foreach (var field in type.GetFields(BindingFlags.Static | BindingFlags.NonPublic))
                {
                    var attribute = field.GetCustomAttribute<PluginEventDelegate>();

                    if (attribute == null)
                        continue;

                    if (DelegateFields.ContainsKey(attribute.ServerEvent))
                    {
                        // log duplicate
#warning LOG ERROR
                        Console.WriteLine("duplicate!");
                        continue;
                    }

                    DelegateFields.Add(attribute.ServerEvent, field);
                    EventIsMultiReturn.Add(attribute.ServerEvent, field.FieldType == typeof(MultiReturnEvent));
                }
            }
        }

        // for time purposes, i will be excluding the "plugin" parameters
        // and their checks, and so that this is more readable.
        // 
        // it is also more difficult,
        // but the concept is still the same.
        public static void RegisterEvents(object instance)
        {
            var type = instance?.GetType();

            if (type == null)
                return;

            foreach (var method in type.GetMethods())
            {
                if (method.IsStatic)
                    continue;

                var attribute = method.GetCustomAttribute<PluginEvent>();

                if (attribute == null)
                    continue;

                if (!EventIsMultiReturn.TryGetValue(attribute.ServerEvent, out var isMultiReturn))
                {
                    #warning LOG ERROR
                    continue;
                }

                if (!DelegateFields.TryGetValue(attribute.ServerEvent, out var field))
                {
                    #warning LOG ERROR
                    continue;
                }

                if (isMultiReturn)
                {
                    MultiReturnEvent multiReturn = (MultiReturnEvent)field.GetValue(null);

                    if (!multiReturn.CheckValidDelegate(method, out var delegateType))
                    {
                        #warning LOG ERROR
                        continue;
                    }

                    AddEvent(attribute.ServerEvent, method.CreateDelegate(delegateType, instance));
                }
                else
                {
                    AddEvent(attribute.ServerEvent, method.CreateDelegate(field.FieldType, instance));
                }
            }
        }



        public static void AddEvent<T>(ServerEventType eventType, T method)
            where T : Delegate
        {
            AddEvent(eventType, method as Delegate);
        }

        public static void RemoveEvent<T>(ServerEventType eventType, T method)
            where T : Delegate
        {
            RemoveEvent(eventType, method as Delegate);
        }

        public static void AddEvent(ServerEventType eventType, Delegate method)
        {
            if (!DelegateFields.TryGetValue(eventType, out var field))
                throw new Exception();

            object fieldVal = field.GetValue(null);

            switch (fieldVal)
            {
                case MultiReturnEvent multiReturn:
                    multiReturn.AddEvent(method);
                    return;

                default:
                    field.SetValue(null, EventHelper.ModifyEvent(fieldVal as Delegate, method, true));
                    return;
            }
        }

        public static void RemoveEvent(ServerEventType eventType, Delegate method)
        {
            if (!DelegateFields.TryGetValue(eventType, out var field))
                throw new Exception();

            object fieldVal = field.GetValue(null);

            switch (fieldVal)
            {
                case MultiReturnEvent multiReturn:
                    multiReturn.RemoveEvent(method);
                    return;

                default:
                    field.SetValue(null, EventHelper.ModifyEvent(fieldVal as Delegate, method, false));
                    return;
            }
        }

        public static bool ExecuteEvent(IEventArguments args) => ExecuteEvent<bool>(args);

        public static T ExecuteEvent<T>(IEventArguments args)
        {
            if (!DelegateFields.TryGetValue(args.BaseType, out var field))
                throw new Exception();

            T cancellation = default(T) switch
            {
                bool _ => (T)(object)true,
                _ => default
            };

            bool cancelled = false;

            var fieldVal = field.GetValue(null);

            Delegate[] delegates;

            switch (fieldVal)
            {
                case MultiReturnEvent multiReturn:
                    delegates = multiReturn.GetInvocationList();
                    break;

                case Delegate dgate:
                    delegates = dgate.GetInvocationList();
                    break;

                default:
                    return cancellation;

            }

            foreach (var result in EventHelper.InvokeSafe(delegates, args))
            {
                if (cancelled)
                    continue;

                if (result is bool b)
                {
                    if (b)
                    {
                        continue;
                    }

                    cancellation = (T)result;
                    cancelled = true;
                }
                else if (result is T r)
                {
                    if (r is not IEventCancellation ecd || !ecd.IsCancelled)
                        continue;

                    cancellation = r;
                    cancelled = true;
                }
                else
                {
#warning LOG ERROR
                    //Log.Error($"Plugin &6{invoker.Plugin.FullName}&r passed invalid data type for event &6{invoker.Method.Name}&r.");
                }
            }

            if (cancelled)
            {
                Console.WriteLine($"> {args.BaseType} event was cancelled\n");
            }
            return cancellation;
        }
    }
}
