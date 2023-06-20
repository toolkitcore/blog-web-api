using Microsoft.EntityFrameworkCore;

namespace Api.Infrastructure
{
    public class ApplicationDbContextInitializer
    {
        private readonly ApplicationDbContext _context;

        public ApplicationDbContextInitializer(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task InitializeAsync()
        {
            try
            {
                await _context.Database.MigrateAsync();
            }
            catch (Exception)
            {
                throw;
                // Log.Error(ex, "Migration error");
            }
        }
    }
}