using LibrarySystem.Core.Entitties;
using LibrarySystem.Core.Entitties.Identity;
using LibrarySystem.Core.Repositories.Contract;
using LibrarySystem.Core.Services.Contract;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace LibrarySystem.Infrastructure.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public ReviewService(IUnitOfWork unitOfWork , UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<Review> GetReviewByIdAsync(int id)
        {
            return await _unitOfWork.Repository<Review>().GetByIdAsync(id);
        }

        public async Task<bool> DeleteReviewAsync(int id)
        {
            var review = await _unitOfWork.Repository<Review>().GetByIdAsync(id);

            if (review == null)
                return false;

            _unitOfWork.Repository<Review>().Delete(review);
            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task<bool> UpdateReviewAsync(int id, Review updatedReview)
        {
            var existingReview = await _unitOfWork.Repository<Review>().GetByIdAsync(id);

            if (existingReview == null)
                return false;

            existingReview.Comment = updatedReview.Comment;
            existingReview.Rating = updatedReview.Rating;
            
            _unitOfWork.Repository<Review>().Update(existingReview);
            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task<bool> AddReviewAsync(Review review)
        {
            await _unitOfWork.Repository<Review>().AddAsync(review);
            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task<bool> ValidateUserPermissionAsync(int reviewId, ClaimsPrincipal user)
        {
            var review = await _unitOfWork.Repository<Review>().GetByIdAsync(reviewId);
            if (review == null) return false;

            var email = user.FindFirstValue(ClaimTypes.Email);
            var currentUser = await _userManager.FindByEmailAsync(email);

            if (currentUser == null) return false;

            return review.UserId == currentUser.Id;
        }
    }
}
