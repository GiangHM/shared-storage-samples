using StorageManagementAPI.Entities;
using Azure.Data.Tables;
using AzureTableStorage.Services;
using AzureTableStorage.Settings;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StorageManagementAPI.Services
{
    public class DocTypeTableService : TableStorageServiceBase<DocumentTypeEntity>, IDocTypeTableService
    {

        public DocTypeTableService(IAzureClientFactory<TableServiceClient> azureClientFactory
             , IOptions<TableStorageOption> options) : base(azureClientFactory, options) { }

        public async Task<DocumentTypeEntity> AddEntity(DocumentTypeEntity entity)
        {
            return await base.InsertOrUpadteEntityAsync(entity);
        }

        public Task<IEnumerable<DocumentTypeEntity>> GetAllData()
        {
            return GetAll();
        }

        public override string GetTableName()
        {
            return "DocumentTypeTable";
        }
    }
}
