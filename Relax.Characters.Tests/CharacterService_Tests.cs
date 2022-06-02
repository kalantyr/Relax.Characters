using System.Threading;
using System.Threading.Tasks;
using Kalantyr.Auth.Client;
using Kalantyr.Web;
using Moq;
using NUnit.Framework;
using Relax.Characters.Models;
using Relax.Characters.Services.Impl;

namespace Relax.Characters.Tests
{
    public class CharacterService_Tests
    {
        private readonly Mock<IAppAuthClient> _authClient = new();

        [Test]
        public async Task Create_Test()
        {
            _authClient
                .Setup(ac => ac.GetUserIdAsync("111", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ResultDto<uint> { Error = new Error{Code = "AuthError"} });
            _authClient
                .Setup(ac => ac.GetUserIdAsync("222", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ResultDto<uint> { Result = 222 });

            var service = new CharacterService(_authClient.Object);
            var result = await service.CreateCharacterAsync(new CharacterInfo(), "111", CancellationToken.None);
            Assert.AreEqual("AuthError", result.Error.Code);
        }
    }
}
