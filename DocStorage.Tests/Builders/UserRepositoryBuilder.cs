using DocStorage.Domain.Users;
using DocStorage.Repository.Users;
using Moq;

namespace DocStorage.Tests.Builders
{
    public static class UserRepositoryBuilder
    {
        public static Mock<IUserRepository> Build(this Mock<IUserRepository> mock)
        {
            mock = new Mock<IUserRepository>().Setup();

            return mock;
        }

        private static Mock<IUserRepository> Setup(this Mock<IUserRepository> mock)
        {
            var userId = Guid.Parse("0769c29c-5d71-49c8-8685-08ab1bf0b922");
            mock.Setup(f => f.Get(userId)).ReturnsAsync(new User
            {
                Id = userId,
                Username = "admin",
                Role = Domain.ValueObjects.Role.Admin
            });

            mock.Setup(f => f.AuthorizeUser(new User
            {
                Username = "admin",
                Password = "admin"

            })).ReturnsAsync(new User
            {
                Id = userId,
                Username = "admin",
                Password = "$2a$11$Fp1asDWq2.gKvkidTuyZRO6H/C0SUJlXX4ndHl/6M/fduqJQFgn8i",
                Role = Domain.ValueObjects.Role.Admin
            });

            mock.Setup(f => f.Update(It.IsAny<IUser>())).ReturnsAsync(new User
            {
                Id = userId,
                Username = "admin",
                Role = Domain.ValueObjects.Role.Regular
            });

            mock.Setup(f => f.Add(It.IsAny<IUser>())).ReturnsAsync(new User
            {
                Id = userId,
                Username = "admin",
                Password = "admin",
                Role = Domain.ValueObjects.Role.Admin
            });

            mock.Setup(f => f.Delete(userId));

            return mock;
        }

    }
}
