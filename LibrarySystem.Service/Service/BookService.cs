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
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublisherService _publisherService;

        public BookService(IUnitOfWork unitOfWork , IPublisherService publisherService)
        {
            _unitOfWork = unitOfWork;
            _publisherService = publisherService;
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
           var book = await GetBookByIdAsync(id);
            if (book == null)
                return false;

            _unitOfWork.Repository<Book>().Delete(book); 
            var result = await _unitOfWork.CompleteAsync();
            return result > 0;
        }
        public async Task<Book> ExistsBookAsync(string Title)
        {
            return await _unitOfWork.Repository<Book>().GetByEntitySpecAsync(new ExistsSpecification<Book>(Title));
        }
        public async Task<Book> GetBookByIdAsync(int id)
        {
            var spec = new BookWithAdditionalInfoSpecification(id);
            return await _unitOfWork.Repository<Book>().GetByEntitySpecAsync(spec);
        }
        public async Task<bool> AddBookWithPublishersAsync(Book book, List<string> publisherNames)
        {
            foreach (var publisherName in publisherNames)
            {
                var existingPublisher = await _publisherService.ExistsPublisherAsync(publisherName);

                if (existingPublisher == null)
                {
                    var newPublisher = new Publisher { FullName = publisherName };
                    await _unitOfWork.Repository<Publisher>().AddAsync(newPublisher);
                    await _unitOfWork.CompleteAsync(); 
                    existingPublisher = newPublisher; 
                }

                book.BookPublishers.Add(new BookPublisher
                {
                    BookId = book.Id,
                    PublisherId = existingPublisher.Id
                });
            }

            await _unitOfWork.CompleteAsync();

            return true; 
        }
        public async Task<bool> UpdateBookAsync(int id, Book updatedBook, List<string> publisherNames)
        {
            var book = await GetBookByIdAsync(id);
            if (book == null)
                return false;

            book.Title = updatedBook.Title;
            book.Description = updatedBook.Description;
            book.Price = updatedBook.Price;
            book.PictureUrl = updatedBook.PictureUrl;
            book.Category = updatedBook.Category;
            book.Auther = updatedBook.Auther;
            book.AutherId = updatedBook.AutherId;
            book.AdditionalInfo = updatedBook.AdditionalInfo;

            book.BookPublishers.Clear();

            foreach (var publisherName in publisherNames)
            {
                var existingPublisher = await _publisherService.ExistsPublisherAsync(publisherName);
                if (existingPublisher == null)
                {
                    var newPublisher = new Publisher { FullName = publisherName };
                    await _unitOfWork.Repository<Publisher>().AddAsync(newPublisher);
                    await _unitOfWork.CompleteAsync(); 
                    existingPublisher = newPublisher;
                }

                book.BookPublishers.Add(new BookPublisher
                {
                    BookId = book.Id,
                    PublisherId = existingPublisher.Id
                });

            }
          
            await _unitOfWork.CompleteAsync();

            return true; 
        }

    }
}
