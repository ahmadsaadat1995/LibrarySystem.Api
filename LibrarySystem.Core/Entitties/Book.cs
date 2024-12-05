using LibrarySystem.Core.Entitties.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Core.Entitties
{
    public class Book : BaseEntity<int>
    {
        
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public Category Category { get; set; }

        public Auther Auther { get; set; }
        public int AutherId { get; set; } 
        
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<BookPublisher> BookPublishers { get; set; } = new List<BookPublisher>();
       
        public AdditionalInformation AdditionalInfo { get; set; }

    }
}
