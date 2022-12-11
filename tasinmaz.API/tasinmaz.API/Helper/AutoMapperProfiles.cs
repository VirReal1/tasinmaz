using AutoMapper;
using tasinmaz.API.Dtos;
using tasinmaz.API.Dtos.Log;
using tasinmaz.API.Dtos.Tasinmaz;
using tasinmaz.API.Entities.Concrete;
using tasinmaz.API.Models;

namespace tasinmaz.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Tasinmaz, TasinmazDto>().ReverseMap();
            CreateMap<Log, LogDto>();
            CreateMap<Kullanici, KullaniciForShowDeleteDto>();
            CreateMap<KullaniciForAddUpdateDto, Kullanici>().ReverseMap();
            CreateMap<KullaniciForShowDeleteDto, Kullanici>();
        }
    }
}