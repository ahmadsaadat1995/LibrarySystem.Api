using LibrarySystem.Core.Entitties;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Core.Specifications
{
    public interface ISpecification<T> 
    {
        Expression<Func<T,bool>> Criteria { get; set; }
        List<Expression<Func<T , object>>> Includes { get; set; }
         List<Func<IQueryable<T>, IQueryable<T>>> ComplexIncludes { get; set; }
        Expression<Func<T ,object>> OrderBy { get; set; }
        Expression<Func<T ,object>> OrderByDesc { get; set; }
        int Take {  get; set; }
        int Skip { get; set; }
        bool IsPaginationEnable { get; set; }

    }
}
