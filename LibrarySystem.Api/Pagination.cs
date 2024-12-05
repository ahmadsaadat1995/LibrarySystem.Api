namespace LibrarySystem.Api
{
    public class Pagination<T>
    {

        public Pagination(int count ,int pageSize ,int pageIndex , IReadOnlyList<T> data)
        {
            Count = count;
            PageIndex = pageIndex;
            PageSize = pageSize;
            Data = data;
          

        }
        public int Count { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public IReadOnlyList<T> Data { get; set; }
      
    }
}
