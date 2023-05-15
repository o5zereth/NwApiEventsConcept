namespace NwApiEventsConcept.Example
{
    using NwApiEventsConcept.Core.Attributes;
    using NwApiEventsConcept.Core.Enums;
    using NwApiEventsConcept.Events.Args.Map;
    using NwApiEventsConcept.Events.Args.Server;
    using System;

    public sealed class TestObject
    {
        [PluginEvent(ServerEventType.RoundEndConditionsCheck)]
        public void RoundEndConditionsCheck_attribute(RoundEndConditionsCheckArgs ev)
        {
            Console.WriteLine("RoundEndConditionsCheck");
        }

        [PluginEvent(ServerEventType.RoundEndConditionsCheck)]
        public bool RoundEndConditionsCheck_bool_attribute(RoundEndConditionsCheckArgs ev)
        {
            Console.WriteLine("RoundEndConditionsCheck_bool");
            return true;
        }

        [PluginEvent(ServerEventType.MapGenerated)]
        public void MapGenerated_attribute(MapGeneratedArgs ev)
        {
            Console.WriteLine("MapGenerated");
        }
    }
}
