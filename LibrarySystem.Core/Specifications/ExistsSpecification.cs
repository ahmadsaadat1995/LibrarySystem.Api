using LibrarySystem.Core.Entitties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Core.Specifications
{
    public class ExistsSpecification<T> :BaseSpecification<T> where T : BaseEntity<int>
    {
        public ExistsSpecification(string fullName)
        : base(BuildCriteria(fullName)) { }

        private static Expression<Func<T,bool>> BuildCriteria(string fullName)
        {
            if (typeof(T) == typeof(Auther))
            {
               return BuildAuthorCriteria(fullName);
            }
            else if (typeof(T) == typeof(Publisher))
            {
               return BuildPublisherCriteria(fullName);
            }
            else if (typeof(T) == typeof(Book))
            {
                return BuildBookCriteria(fullName);
            }
           
            else
            {
               return _ => false;
            }
        }
        
        private static Expression<Func<T,bool>> BuildAuthorCriteria(string fullName)
        {
            var paramter = Expression.Parameter(typeof(T), "x");
            var properity = Expression.Property(paramter, nameof(Auther.FullName));
            var constant = Expression.Constant(fullName);
            var equalsExpression = Expression.Equal(properity, constant);
            return Expression.Lambda<Func<T, bool>>(equalsExpression, paramter);
        }
        private static Expression<Func<T, bool>> BuildPublisherCriteria(string fullName)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var properity = Expression.Property(parameter, nameof(Publisher.FullName));
            var constant = Expression.Constant(fullName);
            var equalsExpression = Expression.Equal(properity, constant);
            return Expression.Lambda<Func<T, bool>>(equalsExpression,parameter);

        }
        private static Expression<Func<T, bool>> BuildBookCriteria(string title)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var properity = Expression.Property(parameter, nameof(Book.Title));
            var constant = Expression.Constant(title);
            var equalsExpression = Expression.Equal(properity, constant);
            return Expression.Lambda<Func<T, bool>>(equalsExpression, parameter);

        }


    }
}
