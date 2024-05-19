using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Clients;

namespace Application.Clients.Get;

internal class GetClientByIdQueryHandler : IQueryHandler<GetClientByIdQuery, GetClientByIdResponse>
{
    private readonly IClientRepository _clientRepository;

    public GetClientByIdQueryHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<Result<GetClientByIdResponse>> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetAsync(request.Id);

        if (client is null)
        {
            return Result.Failure<GetClientByIdResponse>(ClientErrors.ClientNotFound(request.Id));
        }

        return new GetClientByIdResponse(
            client.Id,
            client.Name.Value,
            client.Phone.Prefix,
            client.Phone.Value,
            client.Gender.Value);
    }
}
