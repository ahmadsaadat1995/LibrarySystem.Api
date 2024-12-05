using LibrarySystem.Api.Errors;
using LibrarySystem.Repository.Data.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Api.Controllers
{
    public class BuggyController : ApiControllerBase
    {
        private readonly LibraryDbContext _libraryDbContext;

        public BuggyController(LibraryDbContext libraryDbContext)
        {
                _libraryDbContext = libraryDbContext;
        }
        [HttpGet("NotFound")]
        public ActionResult GetNotFoundRequest()
        {
            var book = _libraryDbContext.Books.Find(100);
            if (book is null)
                return NotFound(new ApiResponse(404,"Not Found"));
            return Ok(book);
        }
        [HttpGet("ServerError")]
        public ActionResult GetServerError()
        {
            var book = _libraryDbContext.Books.Find(100);
            var bookReturned = book.ToString();
            //Will throw Exception Null Refrence Expception 
            return Ok(bookReturned);
        }
        [HttpGet("BadRequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }
        [HttpGet("BadRequest/{id}")]
        public ActionResult GetGetBadRequest(int id)
        {
            if (id < 0)
                return BadRequest(new ApiResponse(400, "Provided Id Must be greater than Zero "));
            return Ok(new { Message = "Valid request!", Id = id });
        }
    }
}
