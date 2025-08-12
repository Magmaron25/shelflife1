using AutoMapper;
using shelflife.data.Entities;
using shelflife.DTO;

namespace shelflife.api.MappingProfiles;

/// <summary>
/// AutoMapper profile that maps between the database entities and the
/// corresponding DTOs used by the API.  Incoming DTOs are mapped to
/// entities and outgoing entities are mapped back to DTOs.
/// </summary>
public class ItemProfile : Profile
{
    public ItemProfile()
    {
        CreateMap<Item, DtoItem>();
        CreateMap<DtoCreateItem, Item>();
        CreateMap<DtoUpdateItem, Item>();
    }
}