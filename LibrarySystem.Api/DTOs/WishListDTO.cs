using LibrarySystem.Core.Entitties.Identity;
using LibrarySystem.Core.Entitties;

namespace LibrarySystem.Api.DTOs
{
    public class WishListDTO
    {

        public int ItemCount => WishlistItems?.Count ?? 0;
        public ICollection<WishlistItem> WishlistItems { get; set; } = new List<WishlistItem>();

    }
}
