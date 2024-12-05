using LibrarySystem.Core.Entitties;
using LibrarySystem.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Core.Repositories.Contract
{
    public interface IGenericRepository<T> where T: BaseEntity<int>
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAllAsyncWithSpec(ISpecification<T> specification);
        Task<T> GetByEntitySpecAsync(ISpecification<T> specification);
        Task<int> GetCountAsyncWithSpec (ISpecification<T> specification);
        Task<T> AddAsync(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}
