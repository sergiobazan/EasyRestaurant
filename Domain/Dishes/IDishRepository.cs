namespace Domain.Dishes;

public interface IDishRepository
{
    void Add(Dish dish);
    Task<Dish?> GetAsync(Guid id);

    Task<List<Dish>> GetByIdsAsync(List<Guid> ids);
}
