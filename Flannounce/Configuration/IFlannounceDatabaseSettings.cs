namespace Flannounce.Configuration
{
    public interface IFlannounceDatabaseSettings
    {
        string FlatsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}