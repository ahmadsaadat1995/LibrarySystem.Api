using LibrarySystem.Core.Entitties;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace LibrarySystem.Core.Specifications
{
    public class BookWithAdditionalInfoSpecification : BaseSpecification<Book> 
    {
        public BookWithAdditionalInfoSpecification(QueryParamsSpec bookSpecParams)
            : base(book => (string.IsNullOrEmpty(bookSpecParams.Search) ||
                           book.Title.ToLower().Contains(bookSpecParams.Search) ||
                           book.Description.ToLower().Contains(bookSpecParams.Search) ||
                           book.Auther.FullName.ToLower().Contains(bookSpecParams.Search)
                           ) &&
                           (string.IsNullOrEmpty(bookSpecParams.AuthorName) ||
                           book.Auther.FullName.ToLower().Contains(bookSpecParams.AuthorName.ToLower())))

        {
            Includes.Add(b => b.AdditionalInfo);
            Includes.Add(b => b.Auther);
          //  Includes.Add(b => b.BookPublishers);
            ComplexIncludes.Add(query => query.Include(b => b.BookPublishers)
                                               .ThenInclude(bp => bp.Publisher));
            if (!string.IsNullOrEmpty(bookSpecParams.Sort))
            {
                switch (bookSpecParams.Sort)
                {
                    case "PriceDesc" :
                        AddOrderDesc(book => book.Price);
                        break;
                    case "Price":
                        AddOrderBy(book => book.Price);
                        break;
                    default:
                        AddOrderBy(book => book.Title);
                        break;
                }
            }
            ApplyPagination((bookSpecParams.PageIndex - 1) * bookSpecParams.PageSize, bookSpecParams.PageSize); //4 (skip 10: take : 20)
        }


        public BookWithAdditionalInfoSpecification(int id ): base(book => book.Id ==id)
        {

            Includes.Add(b => b.AdditionalInfo);
            Includes.Add(b => b.Auther);

            // Includes.Add(b => b.BookPublishers);

            ComplexIncludes.Add(query => query.Include(b => b.BookPublishers)
                                               .ThenInclude(bp => bp.Publisher));

        }
    }
}
