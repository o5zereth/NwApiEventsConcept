namespace NwApiEventsConcept.Core.Delegates
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class MultiReturnEvent
    {
        public MultiReturnEvent(Type eventArgsParam, params Type[] allowedReturnTypes)
        {
            if (allowedReturnTypes.Length == 0)
                throw new ArgumentException("There must be atleast one allowed return type.");

            EventArgsParameterType = eventArgsParam ?? throw new ArgumentException("There must be an event args parameter.");


            Delegates = new Delegate[allowedReturnTypes.Length];
            DelegateTypes = new Type[allowedReturnTypes.Length];

            var dict = new Dictionary<Type, int>();

            for (int i = 0; i < allowedReturnTypes.Length; i++)
            {
                var returnType = allowedReturnTypes[i];

                dict.Add(returnType, i);
                DelegateTypes[i] = CreateGenericDelegate(returnType, eventArgsParam);
            }

            TypeToIndex = dict;
        }

        public readonly Type EventArgsParameterType;

        private readonly IReadOnlyDictionary<Type, int> TypeToIndex;

        private readonly Delegate[] Delegates;

        private readonly Type[] DelegateTypes;

        public void AddEvent(Delegate method)
        {
            GetReturnTypeIndex(method, out var index);

            EventHelper.ModifyEventByReference(ref Delegates[index], method, true);
        }

        public void RemoveEvent(Delegate method)
        {
            GetReturnTypeIndex(method, out var index);

            EventHelper.ModifyEventByReference(ref Delegates[index], method, false);
        }

        public void GetReturnTypeIndex(Delegate method, out int index)
        {
            var returnType = method.Method.ReturnType;

            if (!ParametersEqual(method))
                throw new ArgumentException("Delegate has incompatible parameter types.");

            if (!TypeToIndex.TryGetValue(returnType, out index))
                throw new ArgumentException("Delegate has incompatible return type.");
        }

        public Delegate[] GetInvocationList()
        {
            return Delegates
                .SelectMany(x => x?.GetInvocationList() ?? Array.Empty<Delegate>())
                .ToArray();
        }

        public bool CheckValidDelegate(MethodInfo method, out Type delegateType)
        {
            if (method == null || method.GetParameters().Length != 1)
            {
                delegateType = null;
                return false;
            }

            if (!TypeToIndex.TryGetValue(method.ReturnType, out int index))
            {
                delegateType = null;
                return false;
            }

            delegateType = DelegateTypes[index];
            return true;
        }

        private bool ParametersEqual(Delegate method)
        {
            var param = method?.Method?.GetParameters();

            if (param == null)
                return false;

            if (EventArgsParameterType == null)
            {
                if (param.Length != 0)
                    return false;
            }
            else
            {
                if (param.Length != 1)
                    return false;

                if (!EventArgsParameterType.IsAssignableFrom(param[0].ParameterType))
                    return false;
            }

            return true;
        }

        private Type CreateGenericDelegate(Type returnType, Type arg)
        {
            if (returnType == typeof(void))
            {
                return typeof(Action<>).MakeGenericType(arg);
            }
            else
            {
                return typeof(Func<,>).MakeGenericType(arg, returnType);
            }
        }
    }
}
