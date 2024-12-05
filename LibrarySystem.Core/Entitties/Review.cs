using LibrarySystem.Core.Entitties.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Core.Entitties
{
    public class Review : BaseEntity<int>
    {
        public int BookId { get; set; }
        public Book Book { get; set; }
        public AppUser User { get; set; }
        public string UserId { get; set; }
      
        public string Comment { get; set; }
        public int Rating { get; set; }
     //   public string UserName { get; set; }
      //  public string UserEmail { get; set; }
    }
}
