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
using System.Security.Claims;

namespace LibrarySystem.Api.Controllers
{
    public class WishListController : ApiControllerBase
    {
        private readonly IWishListService _wishListService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public WishListController(IWishListService wishListService, IUnitOfWork unitOfWork , IMapper mapper , UserManager<AppUser> userManager)
        {
            _wishListService = wishListService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }
        [Authorize]
        [HttpPost("{BookId}")]
        public async Task<ActionResult<Wishlist>> AddBookToWish([FromRoute]int BookId)
        {
            var book =await _unitOfWork.Repository<Book>().GetByIdAsync(BookId);
            if (book is null)
                return BadRequest(new ApiResponse(400,"Invalid Data"));
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user =await _userManager.FindByEmailAsync(Email);
            if (await _wishListService.ExistsInWishList(user.Id, BookId))
            {
                return BadRequest(new ApiResponse(400, "The book is already in your wishlist."));
            }
            var wishlist =  await _wishListService.AddBookToWish(Email, book);
            var ReturnedWishList = _mapper.Map<WishListDTO>(wishlist);
            return Ok(ReturnedWishList);
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Wishlist>> GetWishlist()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user =await _userManager.FindByEmailAsync(Email);
            var wishList = await _wishListService.GetWishlistAsync(user.Id);
            if (wishList is null)
                return NotFound(new ApiResponse(404, "Your wishlist is currently empty. Start adding items to build your collection!"));
            var ReturnedWishList = _mapper.Map<WishListDTO>(wishList);
            return Ok(ReturnedWishList);
        }
        [HttpDelete("{BookId}")]
        [Authorize]                  
        public async Task<ActionResult> DeleteBook(int BookId )
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
             var result = await _wishListService.DeleteFromWishlist(user.Id, BookId);
            if (result)
              return Ok(new ApiResponse(200, "Book has been successfully removed from your wishlist."));
           return BadRequest(new ApiResponse(400, "The book was not found in your wishlist."));
            
        }

         

    }
}
