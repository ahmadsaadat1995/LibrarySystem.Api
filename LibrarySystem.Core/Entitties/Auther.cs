using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Core.Entitties
{
    public class Auther : BaseEntity<int>
    {
      
        public string FullName { get; set; }
        public List<Book> Books { get; set; }
        public string Biography { get; set; } 
        public string ProfilePictureUrl { get; set; } 
        public DateTime DateOfBirth { get; set; } 
    }
}
