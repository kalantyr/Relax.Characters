using Kalantyr.Web;

namespace Relax.Characters.Services
{
    public interface IPermissionValidator
    {
        Task<bool> IsAdminAsync(string token, CancellationToken cancellationToken);
    }
}
