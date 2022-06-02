using Kalantyr.Web;
using Microsoft.AspNetCore.Mvc;
using Relax.Characters.Models;
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
            var result = await _characterService.GetCharacterInfoAsync(id, Request.GetAuthToken(), cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("myCharactersIds")]
        public async Task<IActionResult> GetMyCharactersIdsAsync(CancellationToken cancellationToken)
        {
            var result = await _characterService.GetMyCharactersIdsAsync(Request.GetAuthToken(), cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateCharacterAsync(CharacterInfo info, CancellationToken cancellationToken)
        {
            var result = await _characterService.CreateCharacterAsync(info, Request.GetAuthToken(), cancellationToken);
            return Ok(result);
        }
    }
}
