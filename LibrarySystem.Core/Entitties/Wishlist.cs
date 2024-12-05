using LibrarySystem.Core.Entitties.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Core.Entitties
{
    public class Wishlist : BaseEntity<int>
    {
        public ICollection<WishlistItem> WishlistItems { get; set; } = new List<WishlistItem>();
      
        public AppUser User { get; set; }
        public string UserId { get; set; }

        public Wishlist()
        {
            
        }
        public Wishlist(AppUser appUser ,  string userid, WishlistItem book)
        {
            User = appUser;
            UserId = userid;
            WishlistItems.Add(book);
        }


    }
    }
