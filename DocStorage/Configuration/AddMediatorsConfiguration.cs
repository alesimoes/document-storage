using DocStorage.Api.Controllers.Auth;
using DocStorage.Api.Controllers.Documents;
using DocStorage.Api.Controllers.Groups;
using DocStorage.Api.Controllers.Users;
using DocStorage.Application.Adapters;
using DocStorage.Application.Auth;
using DocStorage.Application.Documents;
using DocStorage.Application.Groups;
using DocStorage.Application.Users;
using FluentMediator;

namespace DocStorage.Api.Configuration
{
    public static class AddMediatorsConfiguration
    {
        public static IServiceCollection AddMediators(this IServiceCollection services)
        {
            services.AddFluentMediator(
                builder =>
                {
                    builder.On<AddUserCommand>().PipelineAsync()
                    .Return<User, IUserService>((handler, request) => handler.Add(request.GetUser()));

                    builder.On<UpdateUserCommand>().PipelineAsync()
                   .Return<User, IUserService>((handler, request) => handler.Update(request.GetUser()));

                    builder.On<LoginCommand>().PipelineAsync()
                    .Return<AuthenticatedUser, IAuthService>((handler, request) => handler.Authenticate(request.GetUser()));

                    builder.On<GetUserRequest>().PipelineAsync()
                   .Return<User, IUserService>((handler, request) => handler.Get(request.Id));

                    builder.On<DeleteUserCommand>().PipelineAsync()
                   .Call<IUserService>((handler, request) => handler.Delete(request.Id));

                    builder.On<AddDocumentCommand>().PipelineAsync()
                   .Return<Document, IDocumentService>((handler, request) => handler.Add(request.GetDocument()));

                    builder.On<GetDocumentRequest>().PipelineAsync()
                   .Return<Document, IDocumentService>((handler, request) => handler.Get(request.Id));

                    builder.On<GetGroupRequest>().PipelineAsync()
                   .Return<Group, IGroupService>((handler, request) => handler.Get(request.Id));

                    builder.On<AddGroupCommand>().PipelineAsync()
                   .Return<Group, IGroupService>((handler, request) => handler.Add(request.GetGroup()));

                    builder.On<UpdateGroupCommand>().PipelineAsync()
                   .Return<Group, IGroupService>((handler, request) => handler.Update(request.GetGroup()));

                    builder.On<DeleteGroupCommand>().PipelineAsync()
                    .Call<IGroupService>((handler, request) => handler.Delete(request.Id));

                    builder.On<AddUserInGroupCommand>().PipelineAsync()
                   .Call<IGroupService>((handler, request) => handler.AddUser(request.Id, request.UserId));

                    builder.On<RemoveUserFromGroupCommand>().PipelineAsync()
                   .Call<IGroupService>((handler, request) => handler.RemoveUser(request.Id, request.UserId));

                    builder.On<AddGroupAccessDocumentCommand>().PipelineAsync()
                   .Call<IDocumentAccessService>((handler, request) => handler.AddGroupAccess(request.GetDocumentAccess()));

                    builder.On<RemoveGroupAccessDocumentCommand>().PipelineAsync()
                   .Call<IDocumentAccessService>((handler, request) => handler.RemoveGroupAccess(request.GetDocumentAccess()));

                    builder.On<AddUserAccessDocumentCommand>().PipelineAsync()
                   .Call<IDocumentAccessService>((handler, request) => handler.AddUserAccess(request.GetDocumentAccess()));

                    builder.On<RemoveUserAccessDocumentCommand>().PipelineAsync()
                 .Call<IDocumentAccessService>((handler, request) => handler.RemoveUserAccess(request.GetDocumentAccess()));
                });

            return services;
        }
    }
}
