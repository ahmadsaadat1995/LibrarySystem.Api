using LibrarySystem.Api.Errors;
using LibrarySystem.Api.Helpers;
using LibrarySystem.Core.Entitties;
using LibrarySystem.Core.Entitties.Identity;
using LibrarySystem.Core.Repositories.Contract;
using LibrarySystem.Core.Services.Contract;
using LibrarySystem.Infrastructure.Services;
using LibrarySystem.Repository.Data.Contexts;
using LibrarySystem.Repository.Repositories;
using LibrarySystem.Service.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using ReviewService = LibrarySystem.Infrastructure.Services.ReviewService;

namespace LibrarySystem.Api.Extensions
{
    public static class AppServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services , IConfiguration configuration)
        {

            services.AddScoped<IUnitOfWork ,UnitOfWork>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IPublisherService, PublisherService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IWishListService, WishListService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddScoped<IEmailService, EmailService>();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (ActionContext context) =>
                {
                    var errors = context.ModelState
                                        .Where(d => d.Value.Errors.Count > 0)
                                        .SelectMany(d => d.Value.Errors) 
                                        .Select(e => e.ErrorMessage) 
                                        .ToList(); 

                    var validationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(validationErrorResponse);
                };
            });

            services.AddAutoMapper(typeof(MappingProfiles));
            return services;
        }
    }
}
