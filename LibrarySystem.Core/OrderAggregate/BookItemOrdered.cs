using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Core.OrderAggregate
{
    public  class BookItemOrdered
    {
        public BookItemOrdered()
        {

        }
        public BookItemOrdered(int bookId, string bookTitle, string pictureUrl)
        {
            BookId = bookId;
            BookTitle = bookTitle;
            PictureUrl = pictureUrl;
        }

        public int BookId { get; set; }

        public string BookTitle { get; set; }

        public string PictureUrl { get; set; }

    }
}
