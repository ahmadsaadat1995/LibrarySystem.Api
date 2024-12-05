using LibrarySystem.Core.Entitties;
using LibrarySystem.Core.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Core.Services.Contract
{
    public interface IBookService
    {
        Task<Book> ExistsBookAsync(string Title);
        Task<Book> GetBookByIdAsync(int id);
        Task<bool> DeleteBookAsync(int id);
        Task<bool> AddBookWithPublishersAsync(Book book, List<string> publisherNames);
        Task<bool> UpdateBookAsync(int id, Book updatedBook, List<string> publisherNames);
    }
}
