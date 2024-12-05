using LibrarySystem.Core.Entitties;
using LibrarySystem.Core.Entitties.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Core.Services.Contract
{
    public interface IWishListService
    {
        Task<Wishlist> AddBookToWish(string userId, Book book);
        Task<bool> ExistsInWishList(string userId, int bookId);
        Task<Wishlist> GetWishlistAsync(string userId);
        Task<bool>  DeleteFromWishlist(string userId, int bookId);
    }
}
