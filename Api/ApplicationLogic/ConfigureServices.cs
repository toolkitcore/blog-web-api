using Api.ApplicationLogic.Interface;
using Api.ApplicationLogic.Services;
namespace Api.ApplicationLogic
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            // user
            services.AddScoped<IUserReadService, UserReadService>();
            services.AddScoped<IUserWriteService, UserWriteService>();
            // book
            services.AddScoped<IBookReadService, BookReadService>();
            services.AddScoped<IBookWriteService, BookWriteService>();
            //blog     
            services.AddScoped<IBlogReadService, BlogReadService>();
            services.AddScoped<IBlogWriteService, BlogWriteService>();

            services.AddScoped<ISeedService, SeedService>();

            services.AddSingleton<ICurrentTime, CurrentTime>();

            services.AddScoped<ICacheService, CacheService>();

            return services;
        }
    }
}