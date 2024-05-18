using Infraestructure;
using Microsoft.EntityFrameworkCore;

namespace EasyRestaurant.API.Extentions;

public static class MigrationExtentions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using ApplicationDbContext dbContext =
            scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        dbContext.Database.Migrate();
    }
}
