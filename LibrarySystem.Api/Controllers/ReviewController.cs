using AutoMapper;
using LibrarySystem.Api.DTOs;
using LibrarySystem.Api.Errors;
using LibrarySystem.Core.Entitties;
using LibrarySystem.Core.Entitties.Identity;
using LibrarySystem.Core.Repositories.Contract;
using LibrarySystem.Core.Services.Contract;
using LibrarySystem.Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Security.Claims;
using Review = LibrarySystem.Core.Entitties.Review;

namespace LibrarySystem.Api.Controllers
{
    public class ReviewsController : ApiControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReviewService _reviewService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public ReviewsController(IUnitOfWork unitOfWork, IReviewService reviewService, IMapper mapper , UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _reviewService = reviewService;
            _mapper = mapper;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin,Librarian,User")]
        [HttpPost]
       
        public async Task<ActionResult<ReviewDTO>> AddReviewAsync(ReviewDTO reviewDTO)
        {
            if (reviewDTO == null)
                return BadRequest(new ApiResponse(400, "Invalid review data"));

            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return BadRequest(new ApiResponse(400, "User not found"));

            var review = new Review
            {
                BookId = reviewDTO.BookId,
                UserId =user.Id, // تأكد من تعيين الـ UserId هنا
                Rating = reviewDTO.Rating,
                Comment = reviewDTO.Comment
            };

            // Add the review to the database
            await _unitOfWork.Repository<Review>().AddAsync(review);
            var result = await _unitOfWork.CompleteAsync();

            if (result > 0)
                return Ok(_mapper.Map<Review, ReviewDTO>(review));

            return BadRequest(new ApiResponse(400, "Failed to add review. Please try again later"));
        }

        [Authorize(Roles = "Admin,Librarian,User")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Pagination<ReviewDTO>>>> GetAllReviews([FromQuery] QueryParamsSpec reviewSpecParams)
        {
            var spec = new ReviewSpecification(reviewSpecParams);
            var reviews = await _unitOfWork.Repository<Review>().GetAllAsyncWithSpec(spec);
            var mappedReviews = _mapper.Map<IReadOnlyList<Review>, IReadOnlyList<ReviewDTO>>(reviews);

          var countSpec = new ReviewWithFilterationForCountAsync(reviewSpecParams);
          var count = await _unitOfWork.Repository<Review>().GetCountAsyncWithSpec(countSpec);
          var pagination = new Pagination<ReviewDTO>(count, reviewSpecParams.PageSize, reviewSpecParams.PageIndex, mappedReviews);
          return Ok(pagination);
         
        }

        [Authorize(Roles = "Admin,Librarian,User")]
        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteReview(int Id)
        {
            if (!await _reviewService.ValidateUserPermissionAsync(Id, User))
            {
                return Unauthorized(new ApiResponse(401, "You are not authorized to update this review"));
            }

            var isDeleted = await _reviewService.DeleteReviewAsync(Id);

            if (!isDeleted)
                return BadRequest(new ApiResponse(400, "There was an error deleting the review"));

            return Ok(new ApiResponse(200, "Review deleted successfully"));
        }

        [Authorize(Roles = "Admin,Librarian,User")]
        [HttpPut("{Id}")]
        public async Task<ActionResult<ReviewDTO>> UpdateReview(int Id, [FromBody] ReviewDTO updatedReviewDTO)
        {

            if (!await _reviewService.ValidateUserPermissionAsync(Id, User))
            {
                return Unauthorized(new ApiResponse(401, "You are not authorized to update this review"));
            }

            if (updatedReviewDTO == null)
                return BadRequest(new ApiResponse(400, "Invalid review data"));

            var updatedReview = _mapper.Map<ReviewDTO, Review>(updatedReviewDTO);

            var isUpdated = await _reviewService.UpdateReviewAsync(Id, updatedReview);

            if (!isUpdated)
                return NotFound(new ApiResponse(404, "Review not found"));

            var returnedReview = await _reviewService.GetReviewByIdAsync(Id);
            var returnedReviewDTO = _mapper.Map<Review, ReviewDTO>(returnedReview);

            return Ok(returnedReviewDTO);
        }
    }
}
