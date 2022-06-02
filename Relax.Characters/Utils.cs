using Microsoft.AspNetCore.Mvc;

namespace Relax.Characters
{
    public static class Utils
    {
        public static async Task<ObjectResult> WrapExceptionAsync<T>(Task<T> func)
        {
            try
            {
                return new OkObjectResult(await func);
            }
            catch (Exception e)
            {
                var error = e.GetBaseException();
                return new ObjectResult(error.GetType().Name + ": " + error.Message) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }
    }
}
