using Kalantyr.Auth.Client;
using Microsoft.Extensions.Options;
using Relax.Characters.Config;

namespace Relax.Characters.Services.Impl
{
    public class PermissionValidator: IPermissionValidator
    {
        private readonly IAppAuthClient _authClient;
        private readonly ServiceConfig _serviceConfig;

        public PermissionValidator(IOptions<ServiceConfig> serviceConfig, IAppAuthClient authClient)
        {
            _authClient = authClient ?? throw new ArgumentNullException(nameof(authClient));
            _serviceConfig = serviceConfig.Value;
        }

        public async Task<bool> IsAdminAsync(string token, CancellationToken cancellationToken)
        {
            var getUserIdResult = await _authClient.GetUserIdAsync(token, cancellationToken);
            if (getUserIdResult.Error != null)
                throw new Exception(getUserIdResult.Error.Message);

            return _serviceConfig.AdminUserIds.Contains(getUserIdResult.Result);
        }
    }
}
