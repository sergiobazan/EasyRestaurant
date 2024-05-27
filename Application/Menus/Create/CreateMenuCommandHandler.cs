using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Dishes;
using Domain.Menus;

namespace Application.Menus.Create;

internal class CreateMenuCommandHandler : ICommandHandler<CreateMenuCommand, Guid>
{
    private readonly IMenuRepository _menuRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDishRepository _dishRepository;

    public CreateMenuCommandHandler(IMenuRepository menuRepository, IUnitOfWork unitOfWork, IDishRepository dishRepository)
    {
        _menuRepository = menuRepository;
        _unitOfWork = unitOfWork;
        _dishRepository = dishRepository;
    }

    public async Task<Result<Guid>> Handle(CreateMenuCommand request, CancellationToken cancellationToken)
    {
        List<Dish> dishes = await _dishRepository.GetByIdsAsync(request.DishIds);

        if (dishes.Count == 0)
        {
            return Result.Failure<Guid>(DishErrors.DishesNotFound);
        }

        var result = Menu.Create();

        result.Value.AddDishes(dishes);

        _menuRepository.Add(result.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(result.Value.Id);
    }
}
