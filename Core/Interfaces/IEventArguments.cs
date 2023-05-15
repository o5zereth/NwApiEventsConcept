namespace NwApiEventsConcept.Core.Interfaces
{
    using NwApiEventsConcept.Core.Enums;

    public interface IEventArguments
    {
        ServerEventType BaseType { get; }
    }
}
