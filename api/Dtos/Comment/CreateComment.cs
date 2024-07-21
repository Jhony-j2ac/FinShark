namespace api.Dtos.Comment
{
    public class CreateComment
    {
        public string Title { get; set; } = String.Empty;
        public string Content { get; set; } = String.Empty;
        public int? StockId { get; set; }
    }
}
