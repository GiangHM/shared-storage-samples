using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StorageManagementAPI.Entities
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

    [Table("storagedocument")]
    public class StorageDocument
    {
        public long Id { get; set; }
        public string? DocTypeCode { get; set; }
        public string? DocUrl { get; set; }
        public bool IsActivated { get; set; }
        public DateTime? CreationDate { get; set; }
        public string? CreationUser { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string? ModificationUser { get; set; }
    }
}
