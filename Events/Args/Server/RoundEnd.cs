namespace NwApiEventsConcept.Events.Args.Server
{
    using NwApiEventsConcept.Core.Enums;
    using NwApiEventsConcept.Core.Interfaces;
    using NwApiEventsConcept.Placeholders;

    public sealed class RoundEndConditionsCheckArgs : IEventArguments
    {
        public RoundEndConditionsCheckArgs(bool baseGameConditionsSatisfied)
        {
            BaseGameConditionsSatisfied = baseGameConditionsSatisfied;
        }

        public bool BaseGameConditionsSatisfied { get; }

        public ServerEventType BaseType => ServerEventType.RoundEndConditionsCheck;

        RoundEndConditionsCheckArgs() { }
    }
}
