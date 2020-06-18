namespace Flannounce.Configuration
{
    public class FlannounceDatabaseSettings :IFlannounceDatabaseSettings
    {
        public string AnnouncesCollectionName { get; set; }
        
        public string CleanedAnnouncesCollectionName { get; set; }
        public string StreetsCollectionName { get; set; }
        
        public string DistrictsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}