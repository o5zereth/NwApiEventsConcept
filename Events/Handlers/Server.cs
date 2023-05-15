namespace NwApiEventsConcept.Events.Handlers
{
    using NwApiEventsConcept.Core.Attributes;
    using NwApiEventsConcept.Core.Delegates;
    using NwApiEventsConcept.Core.Enums;
    using NwApiEventsConcept.Events.Args.Server;
    using System;

    public static class Server
    {
        // attribute to identify events
        [PluginEventDelegate(ServerEventType.RoundEndConditionsCheck, typeof(RoundEndConditionsCheckArgs))]
        // make it readonly to prevent assignment
        internal static readonly MultiReturnEvent roundEndConditionsCheck
            = new MultiReturnEvent(
                // required parameter type (can be null)
                typeof(RoundEndConditionsCheckArgs),

                // allowed return types
                typeof(bool),
                typeof(void));


        // Publicly accessible c# events
        // so attributes ARENT required.
        public static event Func<RoundEndConditionsCheckArgs, bool> RoundEndConditionsCheck_bool
        {
            add => roundEndConditionsCheck.AddEvent(value);
            remove => roundEndConditionsCheck.RemoveEvent(value);
        }

        public static event Action<RoundEndConditionsCheckArgs> RoundEndConditionsCheck
        {
            add => roundEndConditionsCheck.AddEvent(value);
            remove => roundEndConditionsCheck.RemoveEvent(value);
        }
    }
}
