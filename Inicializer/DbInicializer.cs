using DemoLinkedInApi.Data;
using Microsoft.EntityFrameworkCore;

namespace DemoLinkedIn.Server.Inicializer;

public class DbInicializer(ApplicationDbContext dbContext) : IDbInicializer
{
    public void Initialize()
    {
        try
        {
            if (dbContext.Database.GetPendingMigrations().Any())
            {
                dbContext.Database.Migrate();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred while migrating the database.");
            throw;
        }
    }
}