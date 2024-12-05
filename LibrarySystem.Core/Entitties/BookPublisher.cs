using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Core.Entitties
{
    public class BookPublisher 
    {
        public int BookId { get; set; }
        public Book Book { get; set; }

        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }
    }
}
