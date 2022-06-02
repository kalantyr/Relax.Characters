using Kalantyr.Web;
using Relax.Characters.InternalModels;
using Relax.Characters.Models;

namespace Relax.Characters.Services.Impl
{
    public class AdminService : IAdminService
    {
        private readonly ICharactersStorageAdmin _storage;
        private readonly IPermissionValidator _permissionValidator;

        public AdminService(ICharactersStorageAdmin storage, IPermissionValidator permissionValidator)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _permissionValidator = permissionValidator ?? throw new ArgumentNullException(nameof(permissionValidator));
        }

        public async Task<ResultDto<bool>> MigrateAsync(string token, CancellationToken cancellationToken)
        {
            var checkResult = await _permissionValidator.IsAdminAsync(token, cancellationToken);
            if (!checkResult)
                return new ResultDto<bool> { Error = Errors.AdminOnlyAccess };

            cancellationToken.ThrowIfCancellationRequested();

            await _storage.MigrateAsync(cancellationToken);
            return ResultDto<bool>.Ok;
        }
    }
}
