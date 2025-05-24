using AzureBlobStorage.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace StorageManagementAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SasTokenGeneratorController(ILogger<SasTokenGeneratorController> _logger
            , IBlobStorageService _blobService) : ControllerBase
    {

        [HttpGet]
        [Route("{containerName}/{fileName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> GenerateSasToken2([FromRoute] string containerName, [FromRoute] string fileName)
        {
            _logger.LogInformation("Get SAS Token");
            var url =  _blobService.CreateServiceSASBlob(containerName
                , fileName
                , 1
                , null
                , Azure.Storage.Sas.BlobContainerSasPermissions.Write);
            return Ok(await Task.FromResult(url));
        }
    }
}
