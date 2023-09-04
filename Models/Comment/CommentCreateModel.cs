namespace Models.Comment
{
    public record CommentCreateModel
    {
        public string Text { get; set; } = string.Empty;
        public int CommenterID { get; set; }
        public int PostID { get; set; }
    }
    public record CommentDTO
    {
        public string Text { get; set; } = string.Empty;
        public int CommenterID { get; set; }
        public int PostID { get; set; }
    }
}