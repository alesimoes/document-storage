using DocStorage.Domain.Groups;
using DocStorage.Repository.Groups;
using Moq;

namespace DocStorage.Tests.Builders
{
    public static class GroupRepositoryBuilder
    {
        public static Mock<IGroupRepository> Build(this Mock<IGroupRepository> mock)
        {
            mock = new Mock<IGroupRepository>().Setup();

            return mock;
        }

        private static Mock<IGroupRepository> Setup(this Mock<IGroupRepository> mock)
        {
            var id = Guid.Parse("0769c29c-5d71-49c8-8685-08ab1bf0b922");
            mock.Setup(f => f.Get(id)).ReturnsAsync(new Group
            {
                Id = id,
                Name = "Group"
            });

            mock.Setup(f => f.Delete(id));

            mock.Setup(f => f.Update(It.IsAny<IGroup>())).ReturnsAsync(new Group
            {
                Id = id,
                Name = "Group"
            });

            mock.Setup(f => f.Add(It.IsAny<IGroup>())).ReturnsAsync(new Group
            {
                Id = id,
                Name = "Group"
            });

            mock.Setup(f => f.AddUser(It.IsAny<Guid>(), It.IsAny<Guid>()));

            mock.Setup(f => f.RemoveUser(It.IsAny<Guid>(), It.IsAny<Guid>()));

            return mock;
        }

    }
}
