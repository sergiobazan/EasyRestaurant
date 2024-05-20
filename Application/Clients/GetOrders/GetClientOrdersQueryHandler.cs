using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Clients;
using Domain.Clients.Responses;

namespace Application.Clients.GetOrders;

internal class GetClientOrdersQueryHandler : IQueryHandler<GetClientOrdersQuery, GetClientOrdersResponse>
{
    private readonly IClientRepository _clientRepository;

    public GetClientOrdersQueryHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<Result<GetClientOrdersResponse>> Handle(GetClientOrdersQuery request, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetAsync(request.CientId);

        if (client is null)
        {
            return Result.Failure<GetClientOrdersResponse>(ClientErrors.ClientNotFound(request.CientId));
        }

        var orders = await _clientRepository.GetOrdersAsync(request.CientId);

        return new GetClientOrdersResponse(
            client.Id,
            client.Name.Value,
            orders);

    }
}
