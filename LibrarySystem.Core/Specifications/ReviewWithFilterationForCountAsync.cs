using LibrarySystem.Core.Entitties;
using LibrarySystem.Core.Specifications;

public class ReviewWithFilterationForCountAsync : BaseSpecification<Review>
{
    public ReviewWithFilterationForCountAsync(QueryParamsSpec reviewSpecParams)
        : base(x =>
            (string.IsNullOrEmpty(reviewSpecParams.Search) || x.Comment.ToLower().Contains(reviewSpecParams.Search.ToLower())) &&
            (!reviewSpecParams.BookId.HasValue || x.BookId == reviewSpecParams.BookId))
    {
    }
}
