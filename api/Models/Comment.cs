using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    [Table("Comments")]
    public class Comment
    {
        public int Id { get; set; }
        public string Title { get; set; } = String.Empty;
        public string Content { get; set; } = String.Empty;

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public int? StockId { get; set; }
        public string AppUserId { get; set; }

        //Navigation property
        public Stock? Stock { get; set; }

        public AppUser AppUser { get; set; }
    }
}
