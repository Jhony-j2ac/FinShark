﻿using api.Dtos.Stock;

namespace api.Dtos.Comment
{
    public class CommentDto
    {
        public int? Id { get; set; } 
        public string Title { get; set; } = String.Empty;
        public string Content { get; set; } = String.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public int? StockId { get; set; }
    }
}