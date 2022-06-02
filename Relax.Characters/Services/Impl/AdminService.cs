using Kalantyr.Web;
using Relax.Characters.InternalModels;

namespace Relax.Characters.Services.Impl
{
    public class AdminService : IAdminService
    {
        private readonly ICharactersStorageAdmin _storage;

        public AdminService(ICharactersStorageAdmin storage)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public async Task<ResultDto<bool>> MigrateAsync(string token, CancellationToken cancellationToken)
        {
            // TODO: проверить права

            await _storage.MigrateAsync(cancellationToken);
            return ResultDto<bool>.Ok;
        }
    }
}
