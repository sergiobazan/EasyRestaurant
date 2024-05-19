using Domain.Abstractions;
using MediatR;

namespace Application.Abstractions.Messaging;

internal interface IQuery<TResponse> : IRequest<Result<TResponse>>
{ }
