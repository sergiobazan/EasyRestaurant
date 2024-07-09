using Domain.Dishes;
using Domain.Shared;

namespace Domain.Orders;

public sealed class PricingService
{
    public PricingDetails CalculatePrice(IEnumerable<Dish> dishes)
    {
        var totalPrice = Price.Zero();

        var isFondo = dishes.Any(dish => dish.DishType == Dishes.Type.Fondo);

        foreach (Dish dish in dishes)
        {
            if (isFondo && dish.DishType == Dishes.Type.Entrada) continue;
            
            totalPrice += dish.Price;
        }

        return new(totalPrice);
    }
}
