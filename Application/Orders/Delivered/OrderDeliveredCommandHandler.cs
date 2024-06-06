using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Orders;
using MediatR;

namespace Application.Orders.Delivered;

internal class OrderDeliveredCommandHandler : ICommandHandler<OrderDeliveredCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublisher _publisher;

    public OrderDeliveredCommandHandler(IUnitOfWork unitOfWork, IOrderRepository orderRepository, IPublisher publisher)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
        _publisher = publisher;
    }

    public async Task<Result> Handle(OrderDeliveredCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetAsync(request.OrderId);

        if (order is null)
        {
            return Result.Failure(OrderErrors.OrderNotFound(request.OrderId));
        }

        var result = order.OrderServed();

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _publisher.Publish(
            new OrderDeliveredEvent(order.Id),
            cancellationToken);

        return Result.Success();
    }
}
