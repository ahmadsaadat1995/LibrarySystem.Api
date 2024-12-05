using LibrarySystem.Core.Entitties;
using LibrarySystem.Core.Repositories.Contract;
using LibrarySystem.Core.Services.Contract;
using LibrarySystem.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Service.Service
{
    public class PublisherService : IPublisherService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PublisherService(IUnitOfWork  unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task AddPublisherToBookAsync(int bookId, List<string> publisherNames)
        {
            throw new NotImplementedException();
        }

        public async Task<Publisher> ExistsPublisherAsync(string fullName)
        {
            return await _unitOfWork.Repository<Publisher>().GetByEntitySpecAsync(new ExistsSpecification<Publisher>(fullName));
        }

        public async Task<IEnumerable<Publisher>> GetAllPublishersAsync(PublisherSpecification publisherWithBooks)
        {
            return await _unitOfWork.Repository<Publisher>().GetAllAsyncWithSpec(publisherWithBooks);
        }



    }
}
