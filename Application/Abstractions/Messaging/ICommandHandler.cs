using Domain.Abstractions;
using MediatR;

namespace Application.Abstractions.Messaging;

internal interface ICommandHanlder<TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{ }

internal interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{ }