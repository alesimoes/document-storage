using DocStorage.Domain.Document;
using DocStorage.Repository.Documents;
using Moq;

namespace DocStorage.Tests.Builders
{
    public static class DocumentAccessRepositoryBuilder
    {
        public static Mock<IDocumentAccessRepository> Build(this Mock<IDocumentAccessRepository> mock)
        {
            mock = new Mock<IDocumentAccessRepository>().Setup();

            return mock;
        }

        private static Mock<IDocumentAccessRepository> Setup(this Mock<IDocumentAccessRepository> mock)
        {
            mock.Setup(f => f.AddUserAccess(It.IsAny<DocumentAccess>()));
            mock.Setup(f => f.RemoveUserAccess(It.IsAny<DocumentAccess>()));
            mock.Setup(f => f.AddGroupAccess(It.IsAny<DocumentAccess>()));
            mock.Setup(f => f.RemoveGroupAccess(It.IsAny<DocumentAccess>()));

            return mock;
        }

    }
}
