namespace NwApiEventsConcept.Core.Attributes
{
    using NwApiEventsConcept.Core.Enums;
    using System;

    public sealed class PluginEventDelegate : Attribute
    {
        public PluginEventDelegate(ServerEventType eventType, Type eventArgsType = null)
        {
            ServerEvent = eventType;
            EventArgs = eventArgsType;
        }

        public ServerEventType ServerEvent { get; }

        public Type EventArgs { get; }
    }
}
