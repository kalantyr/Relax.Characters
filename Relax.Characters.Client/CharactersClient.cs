using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Kalantyr.Web;
using Kalantyr.Web.Impl;
using Relax.Characters.Models;

namespace Relax.Characters.Client
{
    public class CharactersClient : HttpClientBase, ICharactersClient
    {
        private readonly TokenRequestEnricher _tokenRequestEnricher;

        public CharactersClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory, new TokenRequestEnricher())
        {
            _tokenRequestEnricher = (TokenRequestEnricher)RequestEnricher;
        }

        public async Task<ResultDto<CharacterInfo>> GetCharacterInfoAsync(uint characterId, string userToken, CancellationToken cancellationToken)
        {
            _tokenRequestEnricher.Token = userToken;
            return await Get<ResultDto<CharacterInfo>>("/character/info?id=" + characterId, cancellationToken);
        }

        public async Task<ResultDto<IReadOnlyCollection<uint>>> GetMyCharactersIdsAsync(string userToken, CancellationToken cancellationToken)
        {
            _tokenRequestEnricher.Token = userToken;
            return await Get<ResultDto<IReadOnlyCollection<uint>>>("/character/myCharactersIds", cancellationToken);
        }

        public async Task<ResultDto<uint>> CreateAsync(CharacterInfo info, string userToken, CancellationToken cancellationToken)
        {
            _tokenRequestEnricher.Token = userToken;
            return await Post<ResultDto<uint>>("/character/create", Serialize(info), cancellationToken);
        }
    }
}
