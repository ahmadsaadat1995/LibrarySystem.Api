using LibrarySystem.Core.Entitties;
using LibrarySystem.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Core.Services.Contract
{
    public interface IPublisherService
    {
        Task<Publisher> ExistsPublisherAsync(string fullName);
        Task<IEnumerable<Publisher>> GetAllPublishersAsync(PublisherSpecification publisherWithBooks);

        Task AddPublisherToBookAsync(int bookId, List<string> publisherNames);
    }
}
