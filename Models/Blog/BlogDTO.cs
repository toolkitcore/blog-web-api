namespace Models.Blog
{
    public record BlogDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public DateTimeOffset Date { get; set; }
        public ICollection<BlogTopicDTO> BlogTopics { get; set; }
    }
    public record BlogTopicDTO
    {
        public int BlogId { get; set; }
        public int TopicId { get; set; }
    }
}
