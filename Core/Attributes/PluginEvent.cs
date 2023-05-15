namespace NwApiEventsConcept.Core.Attributes
{
    using NwApiEventsConcept.Core.Enums;
    using System;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class PluginEvent : Attribute
    {
        public PluginEvent(ServerEventType eventType)
        {
            ServerEvent = eventType;
        }

        public ServerEventType ServerEvent { get; }
    }
}
