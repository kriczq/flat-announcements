namespace Flannounce.Configuration
{
    public interface IFlannounceDatabaseSettings
    {
        string AnnouncesCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}