using LibrarySystem.Core.Entitties;
using LibrarySystem.Core.Entitties.Identity;
using LibrarySystem.Core.OrderAggregate;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LibrarySystem.Repository.Data.Contexts
{
    public static class LibraryContextSeed
    {
        public static async Task SeedAsync(LibraryDbContext dbContext)
        {
          
            if (!dbContext.Authers.Any())
            {
                var AuthorsData = File.ReadAllText("../LibrarySystem.Repository/DataSeed/Authors.json");
                var Authors = JsonSerializer.Deserialize<List<Auther>>(AuthorsData);
                if (Authors?.Count > 0)
                {
                    foreach (var author in Authors)
                    {
                        await dbContext.Set<Auther>().AddAsync(author);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }

            if (!dbContext.Publishers.Any())
            {
                var PublishersData = File.ReadAllText("../LibrarySystem.Repository/DataSeed/Publishers.json");
                var Publishers = JsonSerializer.Deserialize<List<Publisher>>(PublishersData);
                if (Publishers?.Count > 0)
                {
                    foreach (var publisher in Publishers)
                    {
                        await dbContext.Set<Publisher>().AddAsync(publisher);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }

            if (!dbContext.Books.Any())
            {
                var BooksData = File.ReadAllText("../LibrarySystem.Repository/DataSeed/books.json");
                var Books = JsonSerializer.Deserialize<List<Book>>(BooksData);
                if (Books?.Count > 0)
                {
                    foreach (var Book in Books)
                    {
                        await dbContext.Set<Book>().AddAsync(Book);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }

            if (!dbContext.BookPublishers.Any())
            {
                var BookpublisherData = File.ReadAllText("../LibrarySystem.Repository/DataSeed/BookPublishers.json");
                var Bookpublishers = JsonSerializer.Deserialize<List<BookPublisher>>(BookpublisherData);
                if (Bookpublishers?.Count > 0)
                {
                    foreach (var Bookpublisher in Bookpublishers)
                    {
                        await dbContext.Set<BookPublisher>().AddAsync(Bookpublisher);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }

            if (!dbContext.DeliveryMethods.Any())
            {
                var DeliveryMethodsData = File.ReadAllText("../LibrarySystem.Repository/DataSeed/DeliveryMethods.json");
                var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsData);
                if (DeliveryMethods?.Count > 0)
                {
                    foreach (var deliveryMethod in DeliveryMethods)
                    {
                        await dbContext.Set<DeliveryMethod>().AddAsync(deliveryMethod);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
        }

    }
}
