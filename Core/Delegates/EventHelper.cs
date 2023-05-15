namespace NwApiEventsConcept.Core.Delegates
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    internal static class EventHelper
    {
        private static readonly object _lock = new object();

        // simulates default compiler generated
        // add and remove methods for events.
        //
        // returns the delegate with the operation completed.
        // as reference is not supplied.
        public static T ModifyEvent<T>(T @delegate, Delegate method, bool adding)
            where T : Delegate
        {
            lock (_lock)
            {
                T action = @delegate;
                T action2;
                do
                {
                    action2 = action;
                    T value2 = (T)(adding ? Delegate.Combine(action2, method) : Delegate.Remove(action2, method));
                    action = Interlocked.CompareExchange(ref @delegate, value2, action2);
                }
                while ((object)action != action2);

                return @delegate;
            }
        }

        // simulates default compiler generated
        // add and remove methods for events.
        public static void ModifyEventByReference<T>(ref T field, Delegate method, bool adding)
            where T : Delegate
        {
            lock (_lock)
            {
                T action = field;
                T action2;
                do
                {
                    action2 = action;
                    T value2 = (T)(adding ? Delegate.Combine(action2, method) : Delegate.Remove(action2, method));
                    action = Interlocked.CompareExchange(ref field, value2, action2);
                }
                while ((object)action != action2);
            }
        }

        public static IEnumerable<object> InvokeSafe(Delegate[] delegates, params object[] args)
        {
            for (int i = 0; i < delegates.Length; i++)
            {
                object result;
                try
                {
                    result = delegates[i].DynamicInvoke(args);
                }
                catch (Exception e)
                {
                    result = null;
#warning LOG ERROR
                    Console.WriteLine(e);
                }

                if (result != null)
                {
                    yield return result;
                }
            }
        }
    }
}
