namespace NwApiEventsConcept.Events.Args.Map
{
    using NwApiEventsConcept.Core.Enums;
    using NwApiEventsConcept.Core.Interfaces;

    public sealed class MapGeneratedArgs : IEventArguments
    {
        public ServerEventType BaseType => ServerEventType.MapGenerated;
    }
}
