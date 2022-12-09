using AutoMapper;
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
            CreateMap<Tasinmaz, TasinmazDto>().ForMember(dest => dest.KullaniciId, opt =>
            {
                opt.MapFrom(src => src.Kullanici.Id);
            }).ReverseMap().ForMember(dest => dest.Kullanici.Id, opt =>
            {
                opt.MapFrom(src=>src.KullaniciId);
            });

            CreateMap<Log, LogDto>().ForMember(dest => dest.KullaniciId, opt =>
            {
                opt.MapFrom(src => src.Kullanici.Id);
            }).ReverseMap().ForMember(dest => dest.Kullanici.Id, opt =>
            {
                opt.MapFrom(src => src.KullaniciId);
            });
        }
    }
}