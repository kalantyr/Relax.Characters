using Kalantyr.Web;
using Microsoft.AspNetCore.Mvc;

namespace Relax.Characters.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CharacterController: ControllerBase
    {
        [HttpGet]
        [Route("allCharacters")]
        public async Task<IActionResult> GetAllCharactersAsync(CancellationToken cancellationToken)
        {
            var userToken = Request.GetAuthToken();
            throw new NotImplementedException();
        }
    }
}
