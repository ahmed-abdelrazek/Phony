using AutoMapper;
using Phony.Data;
using Phony.DTOs;
using Phony.Extensions;
using Phony.Models;

namespace Phony
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(x => x.Pass, opt => opt.Ignore())
                .ForMember(x => x.Group, opt => opt.MapFrom(x => MapEnum(x.Group)));
            CreateMap<UserDto, User>()
                .ForMember(x => x.Group, opt => opt.MapFrom(x => (UserGroup)x.Group.Id));

            CreateMap<Client, ClientDto>();
            CreateMap<Store, StoreDto>();
            CreateMap<SalesMan, SalesManDto>();
            CreateMap<Supplier, SupplierDto>();
            CreateMap<Company, CompanyDto>();
            CreateMap<Service, ServiceDto>();
            CreateMap<Item, ItemDto>()
                .ForMember(x => x.Group, opt => opt.MapFrom(x => MapEnum(x.Group)));
            CreateMap<BillItemMove, BillItemMoveDto>();
            CreateMap<BillServiceMove, BillServiceMoveDto>();
            CreateMap<Bill, BillDto>();
        }

        private EnumDto MapEnum(UserGroup userGroup)
        {
            return new EnumDto
            {
                Id = (int)userGroup,
                Name = Enumerations.GetEnumDescription(userGroup)
            };
        }

        private EnumDto MapEnum(ItemGroup itemGroup)
        {
            return new EnumDto
            {
                Id = (int)itemGroup,
                Name = Enumerations.GetEnumDescription(itemGroup)
            };
        }
    }
}
