using Kalantyr.Web;

namespace Relax.Characters.Models
{
    public static class Errors
    {
        public static Error AdminOnlyAccess { get; } = new()
        {
            Code = nameof(AdminOnlyAccess),
            Message = "This action can only be performed by an administrator"
        };

        public static Error CharacterNotFound { get; } = new()
        {
            Code = nameof(CharacterNotFound),
            Message = "Персонаж не найден"
        };
    }
}
