using StorageManagementAPI.Entities;
using StorageManagementAPI.Models;
using AutoMapper;
using Azure.Data.Tables;

namespace StorageManagementAPI
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<DocTypeRequestModel, DocumentTypeEntity>()
                .ForMember(dest => dest.DocumentTypeCode, opt => opt.MapFrom(src => src.DocTypeCode))
                .ForMember(dest => dest.DocumentTypeDescription, opt => opt.MapFrom(src => src.DocTypeDescription))
                .ForMember(dest => dest.PartitionKey, opt => opt.MapFrom(src => src.DocTypeCode))
                .ForMember(dest => dest.RowKey, opt => opt.MapFrom(src => src.DocTypeDescription));

            CreateMap<DocumentTypeEntity, DocTypeResponseModel>()
                .ForMember(dest => dest.DocTypeCode, opt => opt.MapFrom(src => src.DocumentTypeCode))
                .ForMember(dest => dest.DocTypeDescription, opt => opt.MapFrom(src => src.DocumentTypeDescription))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));
        }
    }
}
