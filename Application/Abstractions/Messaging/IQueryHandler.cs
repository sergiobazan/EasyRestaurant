﻿using Domain.Abstractions;
using MediatR;

namespace Application.Abstractions.Messaging;

internal interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{ }