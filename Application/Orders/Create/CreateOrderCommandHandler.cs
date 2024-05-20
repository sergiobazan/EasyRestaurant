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
        List<Dish> dishes = [];

        foreach (var dishId in request.DishIds)
        {
            var dish = await _dishRepository.GetAsync(dishId);
            if (dish is null)
            {
                return Result.Failure<Guid>(DishErrors.DishNotFound(dishId));
            }   
            var decrease = dish.DecreaseQuantity();
            if (decrease.IsFailure)
            {
                return Result.Failure<Guid>(decrease.Error);
            }
            dishes.Add(dish);
        }

        var order = Order.Create(request.ClientId);

        order.Value.AddDishes(dishes);

        _orderRepository.Add(order.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return order.Value.Id;
    }
}
