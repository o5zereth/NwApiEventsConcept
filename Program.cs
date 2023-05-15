namespace NwApiEventsConcept
{
    using NwApiEventsConcept.Core.Enums;
    using NwApiEventsConcept.Events;
    using NwApiEventsConcept.Events.Args.Map;
    using NwApiEventsConcept.Events.Args.Server;
    using NwApiEventsConcept.Events.Handlers;
    using NwApiEventsConcept.Example;
    using System;

    internal static class Program
    {
        public static void Main()
        {
            EventManager.Init();

            // register using csharp events
            Server.RoundEndConditionsCheck += RoundEndConditionsCheck_csharpevent;
            Server.RoundEndConditionsCheck_bool += RoundEndConditionsCheck_bool_csharpevent;
            Map.MapGenerated += MapGenerated_csharpevent;

            var testObj = new TestObject();

            // register using attributes
            //
            // this should only allow instance assignments
            // to prevent issues.
            EventManager.RegisterEvents(testObj);

            // show logs.
            EventManager.ExecuteEvent(new RoundEndConditionsCheckArgs(true));
            EventManager.ExecuteEvent(new MapGeneratedArgs());

            // put some space
            Console.WriteLine();
            Console.WriteLine();


            // unregister attribute events just to prove csharp
            // events are connected.
            Server.RoundEndConditionsCheck -= testObj.RoundEndConditionsCheck_attribute;
            Server.RoundEndConditionsCheck_bool -= testObj.RoundEndConditionsCheck_bool_attribute;
            Map.MapGenerated -= testObj.MapGenerated_attribute;

            // show logs.
            EventManager.ExecuteEvent(new RoundEndConditionsCheckArgs(true));
            EventManager.ExecuteEvent(new MapGeneratedArgs());


            Console.ReadLine();
        }

        static void RoundEndConditionsCheck_csharpevent(RoundEndConditionsCheckArgs ev)
        {
            Console.WriteLine("RoundEndConditionsCheck_csharpevent");
        }

        static bool RoundEndConditionsCheck_bool_csharpevent(RoundEndConditionsCheckArgs ev)
        {
            Console.WriteLine("RoundEndConditionsCheck_bool_csharpevent");
            return true;
        }

        static void MapGenerated_csharpevent(MapGeneratedArgs ev)
        {
            Console.WriteLine("MapGenerated_csharpevent");
        }
    }
}
