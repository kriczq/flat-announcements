namespace Flannounce.Configuration
{
    public class FlannounceDatabaseSettings :IFlannounceDatabaseSettings
    {
        public string FlatsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}