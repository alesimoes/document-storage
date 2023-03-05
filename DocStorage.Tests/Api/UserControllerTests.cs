using AutoMapper;
using DocStorage.Api;
using DocStorage.Api.Configuration;
using DocStorage.Application.Adapters;
using DocStorage.Application.Auth;
using DocStorage.Application.Users;
using DocStorage.Domain.Users;
using DocStorage.Domain.ValueObjects;
using DocStorage.Repository.Connection;
using DocStorage.Repository.Contracts;
using DocStorage.Service.Authorization;
using DocStorage.Tests.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace DocStorage.Tests.Api
{
    public class UserControllerTests
    {
        private readonly string _tokenAdmin;
        private readonly string _tokenManager;
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        public UserControllerTests()
        {
            var userRepository = new Mock<IUserRepository>().Build();
            _mapper = new MapperConfiguration(cfg => cfg.CreateMapper()).CreateMapper();
            _tokenAdmin = new JwtUtils().GenerateJwtToken(new User
            {
                Id = Guid.NewGuid(),
                Password = "admin",
                Role = Role.Admin,
                Username = "admin",
            });

            _tokenManager = new JwtUtils().GenerateJwtToken(new User
            {
                Id = Guid.NewGuid(),
                Password = "admin",
                Role = Role.Manager,
                Username = "admin",
            });

            _httpClient = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<Startup>()
                .ConfigureTestServices(services =>
                {
                    services.AddScoped<IConnectionFactory, DefaultPostgreConnectionFactory>(f =>
                    {
                        return new DefaultPostgreConnectionFactory("Server=localhost;Database=db;Port=1;User Id=user;Password=user;");
                    });
                    services.AddScoped<IUserService, UserService>(f =>
                     {
                         return new UserService(_mapper, userRepository.Object);
                     });
                    services.AddScoped<IAuthService, AuthService>(f =>
                    {
                        return new AuthService(new JwtUtils(), _mapper, userRepository.Object);
                    });
                }))
                .CreateClient();
        }

        [Fact]
        internal async Task When_Valid_UserId_Then_Returns_User()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenAdmin);
            var response = await _httpClient.GetAsync("api/User/0769c29c-5d71-49c8-8685-08ab1bf0b922");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        internal async Task When_Delete_Valid_UserId_Then_Returns_User()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenAdmin);
            var response = await _httpClient.DeleteAsync("api/User/0769c29c-5d71-49c8-8685-08ab1bf0b922");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task When_Add_User_Then_Return_New_User()
        {
            var userRequest = new User
            {
                Id = Guid.NewGuid(),
                Username = "admin",
                Password = "admin",
                Role = Role.Admin
            };
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenAdmin);
            var content = new StringContent(JsonSerializer.Serialize(userRequest), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/User", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task When_Add_User_Without_Required_Fields_Then_Return_Fail()
        {
            var userRequest = new User
            {
                Id = Guid.NewGuid(),
                Role = Role.Admin
            };
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenAdmin);
            var content = new StringContent(JsonSerializer.Serialize(userRequest), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/User", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task When_Update_User_Then_Return_Updated_User()
        {
            var userRequest = new User
            {
                Id = Guid.NewGuid(),
                Username = "admin",
                Password = "admin",
                Role = Role.Regular
            };
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenAdmin);
            var content = new StringContent(JsonSerializer.Serialize(userRequest), Encoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync("api/User", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task When_Valid_Id_Then_Delete_User()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenAdmin);
            var response = await _httpClient.DeleteAsync("api/User/0769c29c-5d71-49c8-8685-08ab1bf0b922");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task When_User_No_Permission_Then_Delete_Fail()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenManager);
            var response = await _httpClient.DeleteAsync("api/User/75e6412c-e461-49c7-8dee-9c9c74468347");
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task When_User_Has_No_Permission_Then_Create_Fails()
        {
            var userRequest = new User
            {
                Username = "Test",
                Password = "test",
                Role = Role.Admin,
            };
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenManager);
            var content = new StringContent(JsonSerializer.Serialize(userRequest), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/User", content);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
