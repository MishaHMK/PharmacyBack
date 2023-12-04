namespace Pharmacy.Helpers
{
    public class ProductParams
    {
        private const int MaxPageSize = 10;
        public int PageNumber { get; set; } = 1;
        private int _pageSize { get; set; } = 4;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public string? SearchName { get; set; }
        public string? Category { get; set; }

        public string? Sort { get; set; }
        public string? OrderBy { get; set; }
    }
}
