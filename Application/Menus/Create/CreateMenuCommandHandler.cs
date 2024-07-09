using Application.Abstractions;
using Application.Abstractions.Clock;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Menus;
using Domain.Shared;

namespace Application.Menus.Create;

internal class CreateMenuCommandHandler : ICommandHandler<CreateMenuCommand, Guid>
{
    private readonly IMenuRepository _menuRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    public CreateMenuCommandHandler(
        IMenuRepository menuRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _menuRepository = menuRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<Guid>> Handle(CreateMenuCommand request, CancellationToken cancellationToken)
    {
        var result = Menu.Create(
            new Name(request.Menu.Name),
            _dateTimeProvider.UtcNow);

        _menuRepository.Add(result.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(result.Value.Id);
    }
}
