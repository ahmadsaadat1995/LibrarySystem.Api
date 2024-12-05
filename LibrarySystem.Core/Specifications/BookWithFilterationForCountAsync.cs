using LibrarySystem.Core.Entitties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Core.Specifications
{
    public class BookWithFilterationForCountAsync : BaseSpecification<Book> 
    {
        public BookWithFilterationForCountAsync(QueryParamsSpec bookSpecParams)
            : base(book => (string.IsNullOrEmpty(bookSpecParams.Search) ||
                           book.Title.ToLower().Contains(bookSpecParams.Search) ||
                           book.Description.ToLower().Contains(bookSpecParams.Search) ||
                           book.Auther.FullName.ToLower().Contains(bookSpecParams.Search)
                           )&&
                           (string.IsNullOrEmpty(bookSpecParams.AuthorName) ||
                           book.Auther.FullName.ToLower().Contains(bookSpecParams.AuthorName.ToLower())))
                           
                          
        {
            
        }
    }
}
