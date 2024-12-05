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
    public class BaseSpecification<T> :ISpecification<T> where T : BaseEntity<int>
    {
        public Expression<Func<T, bool>> Criteria { get ; set ; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public List<Func<IQueryable<T>, IQueryable<T>>> ComplexIncludes { get; set; } = new List<Func<IQueryable<T>, IQueryable<T>>>();

        public Expression<Func<T, object>> OrderBy { get ; set; }
        public Expression<Func<T, object>> OrderByDesc { get ; set ; }
        public int Take { get; set ; }
        public int Skip { get ; set ; }
        public bool IsPaginationEnable { get; set; }

        public BaseSpecification(Expression<Func<T , bool>> CriteriaExpression)
        {
            Criteria = CriteriaExpression; 
        }
        public void AddComplexInclude(Func<IQueryable<T>, IQueryable<T>> includeExpression)
        {
            ComplexIncludes.Add(includeExpression);
        }

        public BaseSpecification()
        {
            
        }
        public void AddOrderBy(Expression<Func<T ,object>> expression)
        {
            OrderBy = expression;

        }
        public void AddOrderDesc(Expression<Func<T ,object>> expression)
        {
            OrderByDesc = expression;
        }
        public void ApplyPagination(int skip , int take )
        {
            Skip = skip;
            Take = take;
            IsPaginationEnable = true;
        }
      
      
    }
}
