﻿using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Dishes;
using Domain.Menus;
using Domain.Shared;

namespace Application.Dishes.Create;

internal class CreateDishCommandHandler : ICommandHandler<CreateDishCommand, Guid>
{
    private readonly IDishRepository _dishRepository;
    private readonly IMenuRepository _menuRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDishCommandHandler(IDishRepository dishRepository, IUnitOfWork unitOfWork, IMenuRepository menuRepository)
    {
        _dishRepository = dishRepository;
        _unitOfWork = unitOfWork;
        _menuRepository = menuRepository;
    }

    public async Task<Result<Guid>> Handle(CreateDishCommand request, CancellationToken cancellationToken)
    {
        var menu = await _menuRepository.GetAsync(request.Dish.MenuId);

        if (menu is null)
        {
            return Result.Failure<Guid>(MenuErrors.MenuNotFound(request.Dish.MenuId));
        }

        var price = Price.Create(request.Dish.Price);

        if (price.IsFailure)
        {
            return Result.Failure<Guid>(price.Error);
        }

        var quantity = Quantity.Create(request.Dish.Quantity);

        if (quantity.IsFailure)
        {
            return Result.Failure<Guid>(quantity.Error);
        }

        var dish = Dish.Create(
            request.Dish.MenuId,
            new Name(request.Dish.Name),
            price.Value,
            new Description(request.Dish.Description),
            quantity.Value,
            request.Dish.Type);

        menu.AddDish(dish.Value);

        _dishRepository.Add(dish.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(dish.Value.Id);
    }
}
