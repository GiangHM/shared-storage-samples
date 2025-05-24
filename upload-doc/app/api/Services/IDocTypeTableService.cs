using StorageManagementAPI.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StorageManagementAPI.Services
{
    public interface IDocTypeTableService
    {
        Task<DocumentTypeEntity> AddEntity(DocumentTypeEntity entity);
        Task<IEnumerable<DocumentTypeEntity>> GetAllData();
    }
}
