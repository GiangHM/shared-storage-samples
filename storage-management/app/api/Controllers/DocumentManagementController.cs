using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using sharedentities.DBEntities;
using storageapi.Models;
using storageapi.Services;
using storagedal.Infra.efcore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageManagementAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentManagementController(ILogger<SasTokenGeneratorController> _logger
        , StorageDbContext _dbContext
        , IAzureServiceBusHelper _serviceBusHelper) : ControllerBase
    {

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> CreateDocument([FromBody] DocumentCreationRequestModel document)
        {
            _logger.LogInformation("Save new document");
            var requestItem = new StorageDocument
            {
                DocTypeCode = document.DocTypeCode,
                DocUrl = document.DocUrl,
            };
            _dbContext.Documents.Add(requestItem);
            var saved = await _dbContext.SaveChangesAsync();
            return Ok(saved);
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<DocumentResponseModel>>> GetDocument()
        {
            _logger.LogInformation("Get document");

            var docs = await _dbContext.Documents.Select(x => new DocumentResponseModel
            {
                DocTypeCode = x.DocTypeCode,
                DocUrl = x.DocUrl,
                IsActivated = x.IsActivated,
            }).ToListAsync();
            return Ok(docs);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> CreateDocumentWithAsyncProcess([FromBody] DocumentCreationRequestModel document)
        {
            _logger.LogInformation("Save new document with async process pattern");
            
            var requestItem = new StorageDocument
            {
                DocTypeCode = document.DocTypeCode,
                DocUrl = document.DocUrl,
            };
            await _serviceBusHelper.SendMessageAsync(requestItem, "document-creation-queue");
            return Ok(true);
        }
    }
}
