﻿using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Dishes;
using Domain.Orders;
using MediatR;

namespace Application.Orders.Change;

public class ChangeOrderCommandHandler : ICommandHandler<ChangeOrderCommand, Guid>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDishRepository _dishRepository;
    private readonly IPublisher _publisher;

    public ChangeOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IDishRepository dishRepository, IPublisher publisher)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _dishRepository = dishRepository;
        _publisher = publisher;
    }

    public async Task<Result<Guid>> Handle(ChangeOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetAsync(request.ChangeOrderRequest.OrderId);

        if (order is null)
        {
            return Result.Failure<Guid>(OrderErrors.OrderNotFound(request.ChangeOrderRequest.OrderId));
        }

        var oldDish = order.Dishes.FirstOrDefault(c => c.Id == request.ChangeOrderRequest.DishIdOld);

        if (oldDish is null)
        {
            return Result.Failure<Guid>(DishErrors.DishNotFound(request.ChangeOrderRequest.DishIdOld));
        }

        var newDish = await _dishRepository.GetAsync(request.ChangeOrderRequest.DishIdNew);

        if (newDish is null)
        {
            return Result.Failure<Guid>(DishErrors.DishNotFound(request.ChangeOrderRequest.DishIdNew));
        }

        var result = order.UpdateDish(oldDish, newDish);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _publisher.Publish(
            new OrderChangedEvent(order.Id),
            cancellationToken);

        return order.Id;
    }
}
