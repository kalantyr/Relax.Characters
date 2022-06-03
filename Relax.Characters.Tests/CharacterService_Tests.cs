using System.Threading;
using System.Threading.Tasks;
using Kalantyr.Auth.Client;
using Kalantyr.Web;
using Moq;
using NUnit.Framework;
using Relax.Characters.InternalModels;
using Relax.Characters.Models;
using Relax.Characters.Services;
using Relax.Characters.Services.Impl;

namespace Relax.Characters.Tests
{
    public class CharacterService_Tests
    {
        private readonly Mock<IAppAuthClient> _authClient = new();
        private readonly Mock<ICreateCharacterValidator> _createValidator = new();
        private readonly Mock<ICharactersStorage> _characterRepository = new();

        public CharacterService_Tests()
        {
            _createValidator
                .Setup(cv => cv.CanCreateAsync(It.IsAny<CharacterInfo>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ResultDto<bool>.Ok);
        }

        [Test]
        public async Task Create_Error_Test()
        {
            _authClient
                .Setup(ac => ac.GetUserIdAsync("111", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ResultDto<uint> { Error = new Error{Code = "AuthError"} });
            _authClient
                .Setup(ac => ac.GetUserIdAsync("222", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ResultDto<uint> { Result = 222 });

            _createValidator
                .Setup(cv => cv.CanCreateAsync(It.IsAny<CharacterInfo>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ResultDto<bool> { Error = new Error{Code = "ValidationError"} });

            var service = new CharacterService(_authClient.Object, _characterRepository.Object, _createValidator.Object);
            var result = await service.CreateCharacterAsync(new CharacterInfo(), "111", CancellationToken.None);
            Assert.AreEqual("AuthError", result.Error.Code);

            result = await service.CreateCharacterAsync(new CharacterInfo(), "222", CancellationToken.None);
            Assert.AreEqual("ValidationError", result.Error.Code);
        }

        [Test]
        public async Task GetCharacterInfo_Test()
        {
            var service = new CharacterService(_authClient.Object, _characterRepository.Object, _createValidator.Object);
            var result = await service.GetCharacterInfoAsync(123, "111", CancellationToken.None);
            Assert.AreEqual(Errors.CharacterNotFound.Code, result.Error.Code);
        }
    }
}
