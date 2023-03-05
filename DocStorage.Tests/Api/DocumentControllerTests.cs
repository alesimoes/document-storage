using AutoMapper;
using DocStorage.Api;
using DocStorage.Api.Configuration;
using DocStorage.Application.Adapters;
using DocStorage.Application.Documents;
using DocStorage.Domain.Document;
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
    public class DocumentControllerTests
    {
        private readonly string _tokenAdmin;
        private readonly string _tokenManager;
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private const string controller = "Document";
        public DocumentControllerTests()
        {
            var repository = new Mock<IDocumentRepository>().Build();
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
                    services.AddScoped<IDocumentService, DocumentService>(f =>
                    {
                        return new DocumentService(_mapper, repository.Object);
                    });
                    services.AddScoped<IDocumentAccessService, DocumentAccessService>(f =>
                    {
                        return new DocumentAccessService(_mapper, new Mock<IDocumentAccessRepository>().Build().Object);
                    });
                }))
                .CreateClient();
        }

        [Fact]
        public async Task When_Add_Group_Then_Return_New_Ok()
        {
            var request = new DocumentAccess
            {
                Id = Guid.NewGuid(),
                DocumentId = Guid.NewGuid(),
                GroupId = Guid.NewGuid(),
            };

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenAdmin);
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"api/{controller}/{Guid.NewGuid()}/access/group", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task When_Delete_Group_Then_Return_New_Ok()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenAdmin);
            var content = new StringContent(JsonSerializer.Serialize(new { id = Guid.NewGuid() }), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/{controller}/{Guid.NewGuid()}/access/user")
            {
                Content = content
            };
            var response = await _httpClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task When_Add_User_Then_Return_New_Ok()
        {
            var request = new DocumentAccess
            {
                Id = Guid.NewGuid(),
                DocumentId = Guid.NewGuid(),
                GroupId = Guid.NewGuid(),
            };

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenAdmin);
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"api/{controller}/{Guid.NewGuid()}/access/user", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }


        [Fact]
        public async Task When_Delete_User_Then_Return_New_Ok()
        {

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenAdmin);
            var content = new StringContent(JsonSerializer.Serialize(new { Id = Guid.NewGuid() }), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/{controller}/{Guid.NewGuid()}/access/user")
            {
                Content = content
            };
            var response = await _httpClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task When_Add_Without_Required_Fields_Then_Return_Fail()
        {
            var request = new Document
            {
                Id = Guid.NewGuid()
            };

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenAdmin);
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"api/{controller}", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task When_No_Permission_Then_Delete_Fail()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenManager);
            var content = new StringContent(JsonSerializer.Serialize(new { Id = Guid.NewGuid() }), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/{controller}/{Guid.NewGuid()}/access/user")
            {
                Content = content
            };
            var response = await _httpClient.SendAsync(request);
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task When_Has_No_Permission_Then_Add_Fails()
        {
            var request = new DocumentAccess
            {
                Id = Guid.NewGuid(),
                DocumentId = Guid.NewGuid(),
                GroupId = Guid.NewGuid(),
            };

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenManager);
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"api/{controller}/{Guid.NewGuid()}/access/user", content);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
