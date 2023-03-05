using DocStorage.Application.Auth;
using DocStorage.Application.Documents;
using DocStorage.Application.Groups;
using DocStorage.Application.Users;
using DocStorage.Domain.Document;
using DocStorage.Domain.Groups;
using DocStorage.Domain.Users;
using DocStorage.Repository.Connection;
using DocStorage.Repository.Contracts;
using DocStorage.Repository.Documents;
using DocStorage.Repository.Groups;
using DocStorage.Repository.Security;
using DocStorage.Repository.Users;
using DocStorage.Service.Authorization;
using DocStorage.Service.Interfaces;

namespace DocStorage.Api.Configuration
{
    public static class AddServicesConfiguration
    {
        public static IServiceCollection AddServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<IJwtUtils, JwtUtils>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IDocumentAccessService, DocumentAccessService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IDocumentAccessRepository, DocumentAccessRepository>();
            services.AddScoped<IConnectionFactory, DefaultPostgreConnectionFactory>(f =>
            {
                return new DefaultPostgreConnectionFactory(configuration.GetConnectionString("DocStorageDb"));
            });

            services.AddScoped<ISecurityContext, SecurityContext>(f =>
            {
                var httpContextAccessor = f.GetRequiredService<IHttpContextAccessor>();
                var userId = httpContextAccessor.HttpContext.User.FindFirst("Id")?.Value;
                var service = new SecurityContext();
                if (!string.IsNullOrEmpty(userId))
                    service.UserId = Guid.Parse(userId);
                return service;
            });

            return services;
        }
    }
}
