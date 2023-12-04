namespace Pharmacy.Helpers
{
    public class OrderParams
    {
        private const int MaxPageSize = 10;
        public int PageNumber { get; set; } = 1;
        private int _pageSize { get; set; } = 6;
        public string? UserId { get; set; }
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        public string? OrderStatus { get; set; }
        public string? Sort { get; set; }
        public string? OrderBy { get; set; }
    }
}