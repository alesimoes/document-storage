using DocStorage.Repository.Connection;
using DocStorage.Repository.Contracts;

namespace DocStorage.Api.Configuration
{
    public static class StartDatabaseConfiguration
    {
        public static IApplicationBuilder StartDatabase(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<IConnectionFactory>();
                context.CreateInitialDatabase();
            }
            new ConfigureTypeMappings();
            return app;
        }
    }
}
