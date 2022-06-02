using Kalantyr.Web;
using Microsoft.AspNetCore.Mvc;
using Relax.Characters.Services;

namespace Relax.Characters.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CharacterController: ControllerBase
    {
        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService ?? throw new ArgumentNullException(nameof(characterService));
        }

        [HttpGet]
        [Route("info")]
        public async Task<IActionResult> GetCharacterInfoAsync(uint id, CancellationToken cancellationToken)
        {
            var result = await _characterService.GetCharacterInfoAsync(Request.GetAuthToken(), id, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("allCharactersIds")]
        public async Task<IActionResult> GetAllCharactersIdsAsync(CancellationToken cancellationToken)
        {
            var result = await _characterService.GetAllCharactersIdsAsync(Request.GetAuthToken(), cancellationToken);
            return Ok(result);
        }
    }
}
