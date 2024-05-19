using Domain.Abstractions;
using MediatR;

namespace Application.Abstractions.Messaging;

internal interface ICommand : IRequest<Result>, ICommandBase
{ }

internal interface ICommand<TResponse> : IRequest<Result<TResponse>>, ICommandBase
{ }

internal interface ICommandBase { }
