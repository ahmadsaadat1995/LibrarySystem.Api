using AutoMapper;
using LibrarySystem.Api.DTOs;
using LibrarySystem.Api.Errors;
using LibrarySystem.Core.Entitties;
using LibrarySystem.Core.Entitties.Enums;
using LibrarySystem.Core.Repositories.Contract;
using LibrarySystem.Core.Services.Contract;
using LibrarySystem.Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace LibrarySystem.Api.Controllers
{
    public class BookController : ApiControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;

        public BookController(IUnitOfWork unitOfWork , IBookService bookService , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _bookService = bookService;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin,Librarian")]
        [HttpPost]
       
        public async Task<ActionResult<BookDTO>> AddBookAsync(BookDTO bookDTO)
        {
            if (bookDTO == null)
                return BadRequest(new ApiResponse(400, "Invalid book data"));

            var existingBook = await _bookService.ExistsBookAsync(bookDTO.Title);

            if (existingBook != null)
                return BadRequest(new ApiResponse(400, "Book already exists"));

           
            var book = _mapper.Map<BookDTO, Book>(bookDTO);

            await _unitOfWork.Repository<Book>().AddAsync(book);
            var result = await _unitOfWork.CompleteAsync();

            var isBookAdded = await _bookService.AddBookWithPublishersAsync(book, bookDTO.Publishers);

            if (!isBookAdded)
                return BadRequest(new ApiResponse(400, "Failed to add book. Please try again later"));

            var returnedBookDTO = _mapper.Map<Book, BookDTO>(book);

            if (result > 0)
                return Ok(returnedBookDTO);

            return BadRequest(new ApiResponse(400, "Failed to add book. Please try again later"));
        }


        [Authorize(Roles = "Admin,Librarian,User")]
        [HttpGet("{Id}")]
        public async Task<ActionResult<BookDTO>> GetBook([FromRoute]int Id)
        {
            var book = await _bookService.GetBookByIdAsync(Id);
            if (book is null)
                return NotFound(new ApiResponse(404, "There is no book with this Id"));
            var mappedBook = _mapper.Map<Book, BookDTO>(book);
            return Ok(mappedBook);
        }



        [Authorize(Roles = "Admin,Librarian,User")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Pagination<BookDTO>>>> GetAllBooks([FromQuery]QueryParamsSpec bookSpecParams)
        {
            var spec = new BookWithAdditionalInfoSpecification(bookSpecParams);
            var books = await _unitOfWork.Repository<Book>().GetAllAsyncWithSpec(spec);
            var Mappedbooks = _mapper.Map<IReadOnlyList<Book>, IReadOnlyList<BookDTO>>(books);
            var countSpec = new BookWithFilterationForCountAsync(bookSpecParams);
            var count = await _unitOfWork.Repository<Book>().GetCountAsyncWithSpec(countSpec);
            var pagination = new Pagination<BookDTO>(count,bookSpecParams.PageSize, bookSpecParams.PageIndex, Mappedbooks);
            return Ok(pagination);
        }


        [Authorize(Roles = "Admin,Librarian")]
        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteBook(int Id)
        {
            var isDeleted = await _bookService.DeleteBookAsync(Id);  

            if (!isDeleted)
                return NotFound(new ApiResponse(404, "There is no book with this Id"));

            return Ok(new ApiResponse(200, "Book deleted successfully"));
        }


        [Authorize(Roles = "Admin,Librarian")]
        [HttpPut("{id}")]
        public async Task<ActionResult<BookDTO>> UpdateBook(int id, [FromBody] BookDTO updatedBookDTO)
        {
            if (updatedBookDTO == null)
                return BadRequest(new ApiResponse(400, "Invalid book data"));

            var updatedBook = _mapper.Map<BookDTO, Book>(updatedBookDTO);

            var isUpdated = await _bookService.UpdateBookAsync(id, updatedBook, updatedBookDTO.Publishers);

            if (!isUpdated)
                return NotFound(new ApiResponse(404, "Book not found"));

            var returnedBook = await _bookService.GetBookByIdAsync(id);
            var returnedBookDTO = _mapper.Map<Book, BookDTO>(returnedBook);

            return Ok(returnedBookDTO);
        }


    }
}
