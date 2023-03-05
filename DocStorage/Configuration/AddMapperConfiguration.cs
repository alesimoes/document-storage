using AutoMapper;
using DocStorage.Application.Adapters;
using DocStorage.Domain.Document;
using DocStorage.Domain.Groups;
using DocStorage.Domain.Users;

namespace DocStorage.Api.Configuration
{
    public static class AddMapperConfiguration
    {
        public static IMapperConfigurationExpression CreateMapper(this IMapperConfigurationExpression cfg)
        {
            //User
            cfg.CreateMap<User, IUser>();
            cfg.CreateMap<Repository.Users.User, User>();
            cfg.CreateMap<IUser, User>();

            //Group
            cfg.CreateMap<Group, IGroup>();
            cfg.CreateMap<Repository.Groups.Group, Group>();
            cfg.CreateMap<IGroup, Group>();

            //Document
            cfg.CreateMap<Document, IDocument>();
            cfg.CreateMap<Repository.Documents.Document, Document>();
            cfg.CreateMap<IDocument, Document>();


            //DocumentAccess
            cfg.CreateMap<DocumentAccess, IDocumentAccess>();
            cfg.CreateMap<Repository.Documents.DocumentAccess, DocumentAccess>();
            cfg.CreateMap<IDocumentAccess, DocumentAccess>();

            return cfg;
        }
    }
}
