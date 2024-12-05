using LibrarySystem.Core.Entitties;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Core.Specifications
{
    public class PublisherSpecification : BaseSpecification<Publisher>
    {
        public PublisherSpecification() : base()
        {
            AddComplexInclude(query => query.Include(p => p.BookPublishers)
                                             .ThenInclude(bp => bp.Book));
        }
    }

}
