using Phony.Data;
using Phony.DTOs;
using Phony.Extensions;
using Phony.Models;
using Riok.Mapperly.Abstractions;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Phony.Mappers
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public static partial class AppMapper
    {
        // Password hashes must never be exposed by model-to-DTO mappings,
        // including users nested in Creator and Editor properties.
        [MapperIgnoreTarget(nameof(UserDto.Pass))]
        public static partial UserDto? ToDto(User? source);

        public static partial User? ToModel(UserDto? source);

        public static partial ClientDto? ToDto(Client? source);
        public static partial StoreDto? ToDto(Store? source);
        public static partial SalesManDto? ToDto(SalesMan? source);
        public static partial SupplierDto? ToDto(Supplier? source);
        public static partial CompanyDto? ToDto(Company? source);
        public static partial ServiceDto? ToDto(Service? source);
        public static partial ItemDto? ToDto(Item? source);
        public static partial BillItemMoveDto? ToDto(BillItemMove? source);

        // These members have no source property. Preserve their existing defaults.
        [MapperIgnoreTarget(nameof(BillServiceMoveDto.QTY))]
        public static partial BillServiceMoveDto? ToDto(BillServiceMove? source);

        [MapPropertyFromSource(nameof(BillDto.Change), Use = nameof(MapChange))]
        [MapProperty(nameof(Bill.ItemsMoves), nameof(BillDto.ItemsMoves), Use = nameof(MapBillItems))]
        [MapProperty(nameof(Bill.ServicesMoves), nameof(BillDto.ServicesMoves), Use = nameof(MapBillServices))]
        public static partial BillDto? ToDto(Bill? source);

        private static decimal MapChange(Bill source) => InvoiceService.Change(source.TotalPayed, source.TotalAfterDiscounts);

        public static partial ObservableCollection<ClientDto> ToClientDtos(IEnumerable<Client> source);
        public static partial ObservableCollection<CompanyDto> ToCompanyDtos(IEnumerable<Company> source);
        public static partial ObservableCollection<ItemDto> ToItemDtos(IEnumerable<Item> source);
        public static partial ObservableCollection<SalesManDto> ToSalesManDtos(IEnumerable<SalesMan> source);
        public static partial List<BillItemMoveDto?> ToBillItemMoveDtos(IEnumerable<BillItemMove?> source);
        public static partial List<BillServiceMoveDto?> ToBillServiceMoveDtos(IEnumerable<BillServiceMove?> source);

        // AutoMapper mapped missing invoice collections to empty lists.
        private static List<BillItemMoveDto?> MapBillItems(List<BillItemMove?>? source)
        {
            var result = new List<BillItemMoveDto?>();
            if (source != null)
            {
                foreach (var move in source)
                {
                    result.Add(ToDto(move));
                }
            }
            return result;
        }

        private static List<BillServiceMoveDto?> MapBillServices(List<BillServiceMove?>? source)
        {
            var result = new List<BillServiceMoveDto?>();
            if (source != null)
            {
                foreach (var move in source)
                {
                    result.Add(ToDto(move));
                }
            }
            return result;
        }

        private static EnumDto ToEnumDto(UserGroup group) => new()
        {
            Id = (int)group,
            Name = Enumerations.GetEnumDescription(group)
        };

        private static EnumDto ToEnumDto(ItemGroup group) => new()
        {
            Id = (int)group,
            Name = Enumerations.GetEnumDescription(group)
        };

        private static UserGroup ToUserGroup(EnumDto group) => (UserGroup)group.Id;
    }
}
