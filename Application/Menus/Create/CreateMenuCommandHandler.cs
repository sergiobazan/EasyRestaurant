using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Menus;
using Domain.Shared;

namespace Application.Menus.Create;

internal class CreateMenuCommandHandler : ICommandHandler<CreateMenuCommand, Guid>
{
    private readonly IMenuRepository _menuRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateMenuCommandHandler(IMenuRepository menuRepository, IUnitOfWork unitOfWork)
    {
        _menuRepository = menuRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateMenuCommand request, CancellationToken cancellationToken)
    {
        var menuDate = MenuDate.Create(request.Menu.Date);

        if (menuDate.IsFailure)
        {
            return Result.Failure<Guid>(menuDate.Error);
        }

        var result = Menu.Create(
            new Name(request.Menu.Name),
            menuDate.Value);

        _menuRepository.Add(result.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(result.Value.Id);
    }
}
