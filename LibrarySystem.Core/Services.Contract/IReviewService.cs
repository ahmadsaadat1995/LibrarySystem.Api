using LibrarySystem.Core.Entitties;
using System.Security.Claims;

namespace LibrarySystem.Core.Services.Contract
{
    public interface IReviewService
    {
        Task<Review> GetReviewByIdAsync(int id);
        Task<bool> AddReviewAsync(Review review);
        Task<bool> UpdateReviewAsync(int id, Review updatedReview);
        Task<bool> DeleteReviewAsync(int id);
        Task<bool> ValidateUserPermissionAsync(int reviewId, ClaimsPrincipal user);
    }
}
