using LibrarySystem.Core.Entitties;
using LibrarySystem.Core.Entitties.Identity;
using LibrarySystem.Core.Repositories.Contract;
using LibrarySystem.Core.Services.Contract;
using LibrarySystem.Core.Specifications;
using LibrarySystem.Repository.Data.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Service.Service
{
    public class WishListService : IWishListService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public WishListService(IUnitOfWork unitOfWork , UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<Wishlist> AddBookToWish(string email, Book book)
        {
            var Appuser = await _userManager.FindByEmailAsync(email);

            var Spec = new WishListWithUserSpecicification(Appuser.Id);
            var wishList = await _unitOfWork.Repository<Wishlist>().GetByEntitySpecAsync(Spec);
            var wishBook = new WishlistItem(book.Id , book.Title , book.Price , book.Category);
               
            if (wishList is null)
            {
                wishList = new Wishlist(Appuser,Appuser.Id, wishBook);
                await _unitOfWork.Repository<Wishlist>().AddAsync(wishList);
            }
            else
            {
                wishList.WishlistItems.Add(wishBook);
            }
            await _unitOfWork.CompleteAsync();
            return wishList;
        }
        public async Task<bool> ExistsInWishList(string userId, int bookId)
        {
            var spec = new WishListWithUserSpecicification(userId, bookId);
            var wishList = await _unitOfWork.Repository<Wishlist>().GetByEntitySpecAsync(spec);
            return wishList != null;
        }
        public async Task<Wishlist> GetWishlistAsync(string userId)
        {
            var Spec = new WishListWithUserSpecicification(userId);
            var wishList = await _unitOfWork.Repository<Wishlist>().GetByEntitySpecAsync(Spec);
            return wishList;
        }
        
        public  async Task<bool> DeleteFromWishlist(string userId , int bookId)
        {
           var BookExists =  await ExistsInWishList(userId , bookId);
            if (BookExists)
            {
                var wishlist = await GetWishlistAsync(userId);
                var spec = new WishListItemSpecification(bookId);
                var wishitem = await _unitOfWork.Repository<WishlistItem>().GetByEntitySpecAsync(spec);
               _unitOfWork.Repository<WishlistItem>().Delete(wishitem);
                await _unitOfWork.CompleteAsync();
                return true;

            }
            else
                return false;
        }

    }

}
