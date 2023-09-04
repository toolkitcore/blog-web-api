namespace Models.Blog
{
    public record BlogUpdateModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public DateTimeOffset Date { get; set; }
        public ICollection<BlogTopicUpdateModel> BlogTopics { get; set; }
    }
    public record BlogTopicUpdateModel
    {
        public int Id { get; set; }
        public int BlogId { get; set; }
        public int TopicId { get; set; }
    }
}
