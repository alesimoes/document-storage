using AutoMapper;
using DocStorage.Api;
using DocStorage.Api.Configuration;
using DocStorage.Application.Adapters;
using DocStorage.Application.Groups;
using DocStorage.Domain.Groups;
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
    public class GroupControllerTests
    {
        private readonly string _tokenAdmin;
        private readonly string _tokenManager;
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private const string controller = "Group";
        public GroupControllerTests()
        {
            var repository = new Mock<IGroupRepository>().Build();
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
                    services.AddScoped<IGroupService, GroupService>(f =>
                    {
                        return new GroupService(_mapper, repository.Object);
                    });
                }))
                .CreateClient();
        }

        [Fact]
        internal async Task When_Valid_Id_Then_Returns_Ok()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenAdmin);
            var response = await _httpClient.GetAsync($"api/{controller}/0769c29c-5d71-49c8-8685-08ab1bf0b922");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        internal async Task When_Delete_Valid_Id_Then_Returns_Ok()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenAdmin);
            var response = await _httpClient.DeleteAsync($"api/{controller}/0769c29c-5d71-49c8-8685-08ab1bf0b922");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task When_Add_Then_Return_New_Ok()
        {
            var request = new Group
            {
                Id = Guid.NewGuid(),
                Name = "Test",
            };

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenAdmin);
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"api/{controller}", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task When_Add_Without_Required_Fields_Then_Return_Fail()
        {
            var request = new Group
            {
                Id = Guid.NewGuid()
            };

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenAdmin);
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"api/{controller}", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task When_Update_Then_Return_Updated_Ok()
        {
            var request = new Group
            {
                Id = Guid.NewGuid(),
                Name = "Tested",
            };
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenAdmin);
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/{controller}", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task When_Valid_Id_Then_Delete_Ok()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenAdmin);
            var response = await _httpClient.DeleteAsync($"api/{controller}/0769c29c-5d71-49c8-8685-08ab1bf0b922");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task When_No_Permission_Then_Delete_Fail()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenManager);
            var response = await _httpClient.DeleteAsync($"api/{controller}/75e6412c-e461-49c7-8dee-9c9c74468347");
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task When_Has_No_Permission_Then_Add_Fails()
        {
            var request = new Group
            {
                Id = Guid.NewGuid(),
                Name = "Tested",
            };
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenManager);
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"api/{controller}", content);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
