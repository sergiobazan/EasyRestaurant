using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Clients;
using Domain.Dishes;
using Domain.Menus;
using Domain.Orders;
using Domain.Shared;
using MediatR;

namespace Application.Orders.Create;

internal class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDishRepository _dishRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IMenuRepository _menuRepository;
    private readonly IPublisher _publisher;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        IDishRepository dishRepository,
        IClientRepository clientRepository,
        IMenuRepository menuRepository,
        IPublisher publisher)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _dishRepository = dishRepository;
        _clientRepository = clientRepository;
        _menuRepository = menuRepository;
        _publisher = publisher;
    }

    public async Task<Result<Guid>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var menu = await _menuRepository.GetAsync(request.MenuId);

        if (menu is null)
        {
            return Result.Failure<Guid>(MenuErrors.MenuNotFound(request.MenuId));
        }

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

        var order = Order.Create(request.ClientId, request.MenuId, new Description(request.Description));

        order.Value.AddDishes(dishes);

        _orderRepository.Add(order.Value);

        menu.AddOrder(order.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _publisher.Publish(
            new OrderCreatedEvent(client.Id),
            cancellationToken);

        return order.Value.Id;
    }
}
