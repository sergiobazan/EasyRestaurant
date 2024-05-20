using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Dishes;
using Domain.Orders;

namespace Application.Orders.Create;

internal class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDishRepository _dishRepository;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IDishRepository dishRepository)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _dishRepository = dishRepository;
    }

    public async Task<Result<Guid>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = Order.Create(request.ClientId);

        List<Dish> dishes = [];

        foreach (var dishId in request.DishIds)
        {
            var dish = await _dishRepository.GetAsync(dishId);
            dishes.Add(dish!);
        }

        order.Value.AddDishes(dishes);

        _orderRepository.Add(order.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return order.Value.Id;
    }
}
