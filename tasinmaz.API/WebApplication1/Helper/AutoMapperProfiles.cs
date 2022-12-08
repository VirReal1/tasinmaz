using AutoMapper;
using tasinmaz.API.Dtos.Tasinmaz;
using tasinmaz.API.Models;

namespace tasinmaz.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<TasinmazDto, Tasinmaz>().ForMember(dest => dest.Kullanici.Id, opt =>
            {
                opt.MapFrom(src => src.KullaniciId);
            });
        }
    }
}