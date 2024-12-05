using LibrarySystem.Api.Errors;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Api.Controllers
{
    [Route("errors/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        public ActionResult Error (int code)
        {
            return NotFound(new ApiResponse(code));
        }

    }
}
