namespace Models.Comment
{
    public class CommentUpdateModel
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public int CommenterID { get; set; }
        public int PostID { get; set; }
    }
}