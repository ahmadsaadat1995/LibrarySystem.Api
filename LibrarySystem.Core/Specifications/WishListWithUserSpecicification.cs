using LibrarySystem.Core.Entitties;
using LibrarySystem.Core.Entitties.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Core.Specifications
{
    public  class WishListWithUserSpecicification : BaseSpecification<Wishlist> 
    {
        public WishListWithUserSpecicification(string userId, int bookId ):base(w => w.UserId == userId && (w.WishlistItems.Any(item => item.BookId == bookId)))
        {
            Includes.Add(w => w.WishlistItems);
            Includes.Add(w => w.User);
        }

        public WishListWithUserSpecicification(string userId)
      : base(w => w.UserId == userId)
        {
            Includes.Add(w => w.User);
            Includes.Add(w => w.WishlistItems);
        }
    }
}
