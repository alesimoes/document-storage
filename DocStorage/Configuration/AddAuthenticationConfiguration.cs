using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace DocStorage.Api.Configuration
{
    public static class AddAuthenticationConfiguration
    {
        public static IServiceCollection AddAuthConfiguration(
            this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            });

            return services;
        }
    }
}
