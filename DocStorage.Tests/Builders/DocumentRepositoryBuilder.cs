using DocStorage.Domain.Document;
using DocStorage.Repository.Documents;
using Moq;

namespace DocStorage.Tests.Builders
{
    public static class DocumentRepositoryBuilder
    {
        public static Mock<IDocumentRepository> Build(this Mock<IDocumentRepository> mock)
        {
            mock = new Mock<IDocumentRepository>().Setup();

            return mock;
        }

        private static Mock<IDocumentRepository> Setup(this Mock<IDocumentRepository> mock)
        {
            var id = Guid.Parse("0769c29c-5d71-49c8-8685-08ab1bf0b922");
            mock.Setup(f => f.Get(id)).ReturnsAsync(new Document
            {
                Id = id,
                Name = "Document",
                Description = "Description",
                Category = "Category",
                Filename = "Filename",
                PostedDate = DateTime.Now,
            });

            mock.Setup(f => f.Add(It.IsAny<Document>())).ReturnsAsync(new Document
            {
                Id = id,
                Name = "Document",
                Description = "Description",
                Category = "Category",
                Filename = "Filename",
                PostedDate = DateTime.Now,
            });
            return mock;
        }

    }
}
