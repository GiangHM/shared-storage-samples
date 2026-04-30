using Azure.Data.Tables;
using Azure;
using System;

namespace storageapi.TableStorageEntities
{
    public class DocumentTypeEntity : ITableEntity
    {
        public DocumentTypeEntity() { }
        public string DocumentTypeCode { get; set; }
        public string DocumentTypeDescription { get; set; }
        public bool IsActive { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
