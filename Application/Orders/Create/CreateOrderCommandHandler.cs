using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Clients;
using Domain.Dishes;
using Domain.Orders;
using Domain.Shared;

namespace Application.Orders.Create;

internal class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDishRepository _dishRepository;
    private readonly IClientRepository _clientRepository;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IDishRepository dishRepository, IClientRepository clientRepository)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _dishRepository = dishRepository;
        _clientRepository = clientRepository;
    }

    public async Task<Result<Guid>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetAsync(request.ClientId);

        if (client is null)
        {
            return Result.Failure<Guid>(ClientErrors.ClientNotFound(request.ClientId));
        }

        List<Dish> dishes = await _dishRepository.GetByIdsAsync(request.DishIds);

        if (dishes.Count == 0)
        {
            return Result.Failure<Guid>(DishErrors.DishesNotFound);
        }

        foreach (var dish in dishes)
        {  
            var decrease = dish.DecreaseQuantity();

            if (decrease.IsFailure)
            {
                return Result.Failure<Guid>(decrease.Error);
            }
        }

        var order = Order.Create(request.ClientId, new Description(request.Description));

        order.Value.AddDishes(dishes);

        _orderRepository.Add(order.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return order.Value.Id;
    }
}
