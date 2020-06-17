namespace Flannounce.Configuration
{
    public class ApiRoutes
    {
        public const string Root = "api";

        public const string Base = Root;

        public static class Announce
        {
            public const string GetAll = Base + "/announce";
            
            public const string GetAvgPricePerCity = Base + "/avgPricePerCity";

            public const string Get = Base + "/announce/{announceId}";

        }
    }
}