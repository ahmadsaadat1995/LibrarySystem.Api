using LibrarySystem.Core.Entitties;

namespace LibrarySystem.Core.Specifications
{
    public class ReviewSpecification : BaseSpecification<Review>
    {
        public ReviewSpecification(QueryParamsSpec reviewSpecParams)
            : base(x =>
                (string.IsNullOrEmpty(reviewSpecParams.Search) || x.Comment.ToLower().Contains(reviewSpecParams.Search.ToLower())) &&
                (!reviewSpecParams.BookId.HasValue || x.BookId == reviewSpecParams.BookId))
        {
            Includes.Add(x => x.User);
           
            ApplyPagination(reviewSpecParams.PageSize * (reviewSpecParams.PageIndex - 1), reviewSpecParams.PageSize);

            if (!string.IsNullOrEmpty(reviewSpecParams.Sort))
            {
                switch (reviewSpecParams.Sort)
                {
                    case "ratingAsc":
                        AddOrderBy(x => x.Rating);
                        break;
                    case "ratingDesc":
                        AddOrderDesc(x => x.Rating);
                        break;
                    default:
                        AddOrderBy(x => x.CreatedAt);
                        break;
                }
            }
        }
    }
}
