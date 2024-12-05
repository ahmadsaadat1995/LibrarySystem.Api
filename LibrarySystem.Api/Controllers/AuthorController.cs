using AutoMapper;
using LibrarySystem.Api.DTOs;
using LibrarySystem.Api.Errors;
using LibrarySystem.Core.Entitties;
using LibrarySystem.Core.Repositories.Contract;
using LibrarySystem.Core.Services.Contract;
using LibrarySystem.Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Api.Controllers
{
    public class AuthorController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthorService _authorService;

        public AuthorController(IMapper mapper , IUnitOfWork unitOfWork , IAuthorService authorService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _authorService = authorService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<AuthorDTO>> AddAuthor ([FromBody] AuthorDTO authorDTO)
        {
            if (authorDTO is null)
                return BadRequest();

            var ExistAuthor = await _authorService.ExistsAuthorAsync(authorDTO.FullName);

            if (ExistAuthor is not null)
                return BadRequest(new ApiResponse(400, "Author already exists."));

            var MappedAuthor = _mapper.Map<AuthorDTO, Auther >(authorDTO);

            await _unitOfWork.Repository<Auther>().AddAsync(MappedAuthor);
             var result = await _unitOfWork.CompleteAsync();
             if(result > 0)
            {
                var ReturnedAuthorDTO = _mapper.Map<Auther,AuthorDTO>(MappedAuthor);
                return Ok(ReturnedAuthorDTO); 
            }

            return BadRequest(new ApiResponse( 400, "Failed to add the author. Please try again."));

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDTO>>> GetAllAuthors()
        {
            var spec = new AuthorSpecification();
            var authers = await _unitOfWork.Repository<Auther>().GetAllAsyncWithSpec(spec);

            if (authers == null || !authers.Any())
                return NotFound(new ApiResponse(404, "No authers found."));
            //var result = authers.Select(a => new AuthorDTO
            //{
            //   Id = a.Id,
            //   FullName = a.FullName,
            //   Books = a.Books.Select(a => a.Title).ToList(),
            //});
            var result = _mapper.Map<IReadOnlyList< Auther>,IReadOnlyList<AuthorDTO>>(authers);
            return Ok(result);
        }



    }
}
