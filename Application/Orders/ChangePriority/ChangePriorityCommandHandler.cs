using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Orders;
using MediatR;

namespace Application.Orders.ChangePriority;

internal class ChangePriorityCommandHandler : ICommandHandler<ChangePriorityCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublisher _publisher;

    public ChangePriorityCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IPublisher publisher)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }

    public async Task<Result> Handle(ChangePriorityCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetAsync(request.OrderId);

        if (order is null)
        {
            return Result.Failure(OrderErrors.OrderNotFound(request.OrderId));
        }

        order.ChangePriority();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _publisher.Publish(
            new OrderPriorityChangedEvent(order.Id),
            cancellationToken);

        return Result.Success();
    }
}
