using Api.Core.Entities;
using AutoMapper;
using Models.Blog;
using Models.Book;
using Models.Comment;
using Models.Topic;
using Models.User;

namespace Api.ApplicationLogic.Mapper
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Book, BookDTO>().ReverseMap();

            CreateMap<User, LoginRequest>().ReverseMap();
            CreateMap<User, RegisterRequest>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();

            CreateMap<Blog, BlogCreateModel>().ReverseMap();
            CreateMap<Blog, BlogUpdateModel>().ReverseMap();
            CreateMap<Blog, BlogDTO>().ReverseMap();

            CreateMap<BlogTopic, BlogTopicCreateModel>().ReverseMap();
            CreateMap<BlogTopic, BlogTopicUpdateModel>().ReverseMap();
            CreateMap<BlogTopic, BlogTopicDTO>().ReverseMap();
            
            CreateMap<Comment, CommentCreateModel>().ReverseMap();
            CreateMap<Comment, CommentUpdateModel>().ReverseMap();
            CreateMap<Comment, CommentDTO>().ReverseMap();

            CreateMap<Topic, TopicDTO>().ReverseMap();
            CreateMap<Topic, TopicCreateModel>().ReverseMap();
            CreateMap<Topic, TopicUpdateModel>().ReverseMap();

        }
    }
}