using LibrarySystem.Core.Entitties;
using LibrarySystem.Core.Repositories.Contract;
using LibrarySystem.Repository.Data.Contexts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryDbContext _libraryDbContext;
        private Hashtable _repositories;
        public UnitOfWork(LibraryDbContext libraryDbContext)
        {
           _libraryDbContext = libraryDbContext;
            _repositories = new Hashtable();
        }
        public async Task<int> CompleteAsync()

        => await _libraryDbContext.SaveChangesAsync();
        

        public ValueTask DisposeAsync()
        => _libraryDbContext.DisposeAsync();

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity<int>
        {
            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                var Repository = new GenericRepository<TEntity>(_libraryDbContext);
                _repositories.Add(type, Repository);
            }
            if (_repositories[type] is  IGenericRepository<TEntity> repoinstence)
            {
                return repoinstence;
            }
            throw new InvalidCastException($"The repository for type {type.Name} is not of the expected type.");
        }
    }
}
