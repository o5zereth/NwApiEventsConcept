namespace NwApiEventsConcept.Events.Handlers
{
    using NwApiEventsConcept.Core.Attributes;
    using NwApiEventsConcept.Core.Delegates;
    using NwApiEventsConcept.Core.Enums;
    using NwApiEventsConcept.Events.Args.Map;
    using System;

    public static class Map
    {
        // attribute to identify events
        [PluginEventDelegate(ServerEventType.MapGenerated)]
        internal static Action<MapGeneratedArgs> mapGenerated;


        // Publicly accessible c# events
        // so attributes ARENT required.
        public static event Action<MapGeneratedArgs> MapGenerated
        {
            add => EventHelper.ModifyEventByReference(ref mapGenerated, value, true);
            remove => EventHelper.ModifyEventByReference(ref mapGenerated, value, false);
        }
    }
}
