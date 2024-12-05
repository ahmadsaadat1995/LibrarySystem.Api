using LibrarySystem.Core.Entitties;
using LibrarySystem.Core.Repositories.Contract;
using LibrarySystem.Core.Specifications;
using LibrarySystem.Repository.Data.Contexts;
using LibrarySystem.Repository.Specification;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibrarySystem.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity<int>
    {
        private readonly LibraryDbContext _libraryDbContext;

        public GenericRepository(LibraryDbContext libraryDbContext)
        {
            _libraryDbContext = libraryDbContext;
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _libraryDbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _libraryDbContext.Set<T>().FindAsync(id);
        }

        public IQueryable<T> ApplySpecification(ISpecification<T> specification)
        {
            return SpecifiactionEvalutor<T>.GetQuery(_libraryDbContext.Set<T>(), specification);
        }

        public async Task<T> AddAsync(T entity)
        {
            await _libraryDbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public void Delete(T entity)
        {
            _libraryDbContext.Remove(entity);
           
        }

        public async Task<IReadOnlyList<T>> GetAllAsyncWithSpec(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).ToListAsync();
        }

        public async Task<T> GetByEntitySpecAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync();
        }

        public async Task<int> GetCountAsyncWithSpec(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).CountAsync();
        }

        public void Update(T entity)
        {
            _libraryDbContext.Update(entity);
          
        }
    }
}
