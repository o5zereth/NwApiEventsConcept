namespace NwApiEventsConcept.Core.Attributes
{
    using NwApiEventsConcept.Core.Enums;
    using System;

    public sealed class PluginEventDelegate : Attribute
    {
        public PluginEventDelegate(ServerEventType eventType)
        {
            ServerEvent = eventType;
        }

        public ServerEventType ServerEvent { get; }
    }
}
