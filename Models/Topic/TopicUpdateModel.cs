namespace Models.Topic
{
    public record TopicUpdateModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
    public record TopicGetsModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}