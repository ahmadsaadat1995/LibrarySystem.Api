using AutoMapper;
using LibrarySystem.Api.DTOs;
using LibrarySystem.Core.Entitties;
using LibrarySystem.Core.Entitties.Identity;
using LibrarySystem.Core.OrderAggregate;
using LibrarySystem.Core.Repositories.Contract;
using LibrarySystem.Repository.Repositories;

namespace LibrarySystem.Api.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Auther, AuthorDTO>()
                //.ForMember(d => d.Books, options => options.
                // MapFrom(d => d.Books.Select(book => new BookDTO
                // {
                //     Id = book.Id,
                //     Title = book.Title,
                // }).ToList()))
                .ForMember(d => d.Books,
                 options => options.MapFrom(src => src.Books.Select(b=> b.Title)))
                .ReverseMap();

            CreateMap<Review, ReviewDTO>()
                .ForMember(d => d.UserEmail,
                options => options.MapFrom(src => src.User.Email))
                .ForMember(d => d.UserName,
                options => options.MapFrom(src => src.User.UserName))
                .ReverseMap();

            CreateMap<Publisher, PublisherDTO>().ReverseMap();

            CreateMap<Book, BookDTO>()
                .ForMember(d => d.AdditionalInfo, options => options.
                MapFrom(book => new AdditionalInformationDTO
                {
                    Language = book.AdditionalInfo.Language,
                    Format = book.AdditionalInfo.Format,
                    DatePublished = book.AdditionalInfo.DatePublished
                }))
                .ForMember(d => d.Publishers , options => options.MapFrom(src => src.BookPublishers.Select(bp => bp.Publisher.FullName).ToList()))
                 .ForMember(d => d.AuthorName, options => options.MapFrom(src => src.Auther.FullName))
                .ReverseMap();

            CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap();
            CreateMap<BasketItem, BasketItemDto>().ReverseMap();
            CreateMap<AdditionalInformationDTO, AdditionalInformation>().ReverseMap();
            CreateMap<AppUser, UserDto>().ReverseMap();
            CreateMap<AddressDto, Core.Entitties.Identity.Address>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<Core.OrderAggregate.Address, AddressDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
            CreateMap<Order, OrderToReturnDto>().ReverseMap();
            CreateMap<Wishlist, WishListDTO>().ReverseMap();
            CreateMap<AppUser, UsersReturnDTO>().ReverseMap();
           CreateMap<BookPublisher, BookPublisherDTO>().ReverseMap();
          
        }

    }
}
