using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Orders;
using MediatR;

namespace Application.Orders.Cancel;

internal class OrderCanceledCommandHandler : ICommandHandler<OrderCanceledCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublisher _publisher;

    public OrderCanceledCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IPublisher publisher)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }

    public async Task<Result> Handle(OrderCanceledCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetAsync(request.OrderId);

        if (order is null)
        {
            return Result.Failure(OrderErrors.OrderNotFound(request.OrderId));
        }

        var result = order.OrderCanceled();

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _publisher.Publish(
            new OrderCanceledEvent(order.Id),
            cancellationToken);

        return Result.Success();
    }
}
