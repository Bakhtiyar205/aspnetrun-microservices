using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;

namespace Ordering.Application.Features.Ordering.Queries.GetOrderList;
public class GetOrderListQuery : IRequest<List<OrdersVm>>
{
    public string? UserName { get; set; }

    public GetOrderListQuery(string? userName)
    {
        UserName = userName ?? throw new ArgumentNullException(nameof(userName));
    }
}

public class GetOrdersListQueryHandler : IRequestHandler<GetOrderListQuery, List<OrdersVm>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public GetOrdersListQueryHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _mapper = mapper;
    }

    public async Task<List<OrdersVm>> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
    {
       var orderList = await _orderRepository.GetOrdersByUserName(request.UserName);
       return _mapper.Map<List<OrdersVm>>(orderList);
    }
}
