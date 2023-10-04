using AutoMapper;
using Ordering.Application.Features.Ordering.Commands.CheckoutOrder;
using Ordering.Application.Features.Ordering.Commands.UpdateOrder;
using Ordering.Application.Features.Ordering.Queries.GetOrderList;
using Ordering.Domain.Entities;

namespace Ordering.Application.Mappings;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Order, OrdersVm>().ReverseMap();
        CreateMap<Order, CheckoutOrderCommand>().ReverseMap();
        CreateMap<Order, UpdateOrderCommand>().ReverseMap();
    }
}
