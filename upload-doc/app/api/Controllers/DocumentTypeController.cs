using StorageManagementAPI.Entities;
using StorageManagementAPI.Models;
using StorageManagementAPI.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageManagementAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentTypeController : ControllerBase
    {
        private readonly ILogger<DocumentTypeController> _logger;
        private readonly IDocTypeTableService _docTypeTableService;
        private readonly IMapper _mapper;


        public DocumentTypeController(ILogger<DocumentTypeController> logger
            , IDocTypeTableService docTypeTableService
            , IMapper mapper)
        {
            _logger = logger;
            _docTypeTableService = docTypeTableService;
            _mapper = mapper;
        }

        [HttpGet()]
        public async Task<IEnumerable<DocTypeResponseModel>> GetAll()
        {
            _logger.LogInformation("Get all docTypes - logging scenario 3");
             var entities = await _docTypeTableService.GetAllData();
                
            var res = _mapper.Map<IEnumerable<DocumentTypeEntity>, IEnumerable<DocTypeResponseModel>>(entities);
            return res;
        }
        [HttpPost()]
        public async Task<bool> CreateNewdocType(DocTypeRequestModel model)
        {
            _logger.LogInformation("Create new docType - logging scenario 3");
            var entity = _mapper.Map<DocumentTypeEntity>(model);
            var res = await _docTypeTableService.AddEntity(entity);
            return true;
        }
        [HttpGet("{code}")]
        public async Task<DocTypeResponseModel> GetByCode([FromRoute]string code)
        {
            _logger.LogInformation("Get by code - logging scenario 3");
            var entities = await _docTypeTableService.GetAllData();
            var entity = entities?.FirstOrDefault(x=> x.DocumentTypeCode == code);
               
            var res = _mapper.Map<DocumentTypeEntity, DocTypeResponseModel>(entity);
            return res;
        }
    }
}
