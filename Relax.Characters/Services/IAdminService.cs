using Kalantyr.Web;

namespace Relax.Characters.Services
{
    public interface IAdminService
    {
        Task<ResultDto<bool>> MigrateAsync(string token, CancellationToken cancellationToken);
    }
}
