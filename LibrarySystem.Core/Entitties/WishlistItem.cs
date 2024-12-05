using LibrarySystem.Core.Entitties.Enums;
using LibrarySystem.Core.Entitties.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LibrarySystem.Core.Entitties
{
    public  class WishlistItem : BaseEntity<int>
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public Category Category { get; set; }

      
        public WishlistItem(int bookId, string title, decimal price, Category category)
        {
            BookId = bookId;
            Title = title;
            Price = price;
            Category = category;
        }
    }
}
