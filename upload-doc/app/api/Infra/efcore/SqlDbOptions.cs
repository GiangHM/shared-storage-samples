namespace storageapi.Infra.efcore
{
    public class SqlDbOptions
    {
        public string ConnectionString { get; set; } = string.Empty;
        public bool EnableDetailedErrors { get; set; }
    }
}
