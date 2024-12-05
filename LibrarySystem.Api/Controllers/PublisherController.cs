using AutoMapper;
using LibrarySystem.Api.DTOs;
using LibrarySystem.Api.Errors;
using LibrarySystem.Core.Entitties;
using LibrarySystem.Core.Repositories.Contract;
using LibrarySystem.Core.Services.Contract;
using LibrarySystem.Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LibrarySystem.Api.Controllers
{
    public class PublisherController : ApiControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPublisherService _publisherService;

        public PublisherController(IUnitOfWork unitOfWork, IMapper mapper, IPublisherService publisherService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _publisherService = publisherService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<PublisherDTO>> AddPublisher([FromBody] PublisherDTO publisherDTO)
        {
            if (publisherDTO is null)
                return BadRequest();

            var ExistPublisher = await _publisherService.ExistsPublisherAsync(publisherDTO.FullName);

            if (ExistPublisher is not null)
                return BadRequest(new ApiResponse(400, "Publisher already exists."));

            var MappedPublisher = _mapper.Map<PublisherDTO, Publisher>(publisherDTO);

            await _unitOfWork.Repository<Publisher>().AddAsync(MappedPublisher);
            var result = await _unitOfWork.CompleteAsync();
            if (result > 0)
            {
                var ReturnedPublisherDTO = _mapper.Map<Publisher, PublisherDTO>(MappedPublisher);
                return Ok(ReturnedPublisherDTO);
            }

            return BadRequest(new ApiResponse(400, "Failed to add the Publisher. Please try again."));

        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
         public async Task<ActionResult<IEnumerable<PublisherDTO>>> GetAllPublishers()
        {
            var spec = new PublisherSpecification();
            var publishers = await _unitOfWork.Repository<Publisher>().GetAllAsyncWithSpec(spec);

            if (publishers == null || !publishers.Any())
                return NotFound(new ApiResponse(404, "No publishers found."));

            var result = publishers.Select(p => new PublisherDTO
            {
                Id = p.Id,
                FullName = p.FullName,
                Books = p.BookPublishers.Select(bp => bp.Book.Title).ToList()
            });
            return Ok(result);
        }


    }
}