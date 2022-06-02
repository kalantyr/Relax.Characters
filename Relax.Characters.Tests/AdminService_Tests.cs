using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Relax.Characters.InternalModels;
using Relax.Characters.Models;
using Relax.Characters.Services;
using Relax.Characters.Services.Impl;

namespace Relax.Characters.Tests
{
    public class AdminService_Tests
    {
        private readonly Mock<IPermissionValidator> _permissionValidator = new();
        private readonly Mock<ICharactersStorageAdmin> _storage = new();

        [Test]
        public async Task Migrate_Test()
        {
            _permissionValidator
                .Setup(pv => pv.IsAdminAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var service = new AdminService(_storage.Object, _permissionValidator.Object);
            var result = await service.MigrateAsync("123", CancellationToken.None);
            Assert.AreEqual(Errors.AdminOnlyAccess.Code, result.Error.Code);
        }
    }
}
