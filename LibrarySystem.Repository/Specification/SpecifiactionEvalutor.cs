using LibrarySystem.Core.Entitties;
using LibrarySystem.Core.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Repository.Specification
{
    public class SpecifiactionEvalutor<T> where T : BaseEntity<int>
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery ,ISpecification<T> specification)
        {
            var query = inputQuery;
            if(specification.Criteria is not null)
            {
                query = query.Where(specification.Criteria);
            }
            if(specification.OrderBy is not null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            if(specification.OrderByDesc is not null)
            {
                query = query.OrderByDescending(specification.OrderByDesc);
            }
            if(specification.IsPaginationEnable)
            {
                query = query.Skip(specification.Skip).Take(specification.Take);
            }


            query = specification.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

            foreach (var complexInclude in specification.ComplexIncludes)
            {
                query = complexInclude(query);
            }
            return query;



        }

       
    }
}
