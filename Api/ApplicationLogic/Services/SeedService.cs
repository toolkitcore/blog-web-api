using Api.ApplicationLogic.Interface;
using Api.Core.Entities;
using Api.Infrastructure;
using Api.Presentation.Constants;
using Serilog;
using System.Text.Json;

namespace Api.ApplicationLogic.Services
{
    public class SeedService : ISeedService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SeedService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Seed()
        {
            if (!await _unitOfWork.BookRepository.AnyAsync())
            {
                string json = File.ReadAllText(LinkConstants.BookData);
                List<Book> books = JsonSerializer.Deserialize<List<Book>>(json)!;
                await _unitOfWork.ExecuteTransactionAsync(() =>
                {
                    _unitOfWork.BookRepository.AddRangeAsync(books);
                });
            };

            await _unitOfWork.TopicRepository.DeleteAllAsync();
            await _unitOfWork.BlogRepository.DeleteAllAsync();
            await _unitOfWork.CommentRepository.DeleteAllAsync();
            await _unitOfWork.UserRepository.DeleteAllAsync();


            // await SeedingTopicData();
            await SeedingUserData();
            await SeedingBlogData();
            // await SeedingCommentData();
        }

        private async Task SeedingUserData()
        {
            if (!await _unitOfWork.UserRepository.AnyAsync())
            {
                string json = File.ReadAllText(LinkConstants.UserData);
                List<User> users = JsonSerializer.Deserialize<List<User>>(json)!;
                await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    await _unitOfWork.UserRepository.AddRangeAsync(users);
                });
                Log.Information("Seeding User data: " + JsonSerializer.Serialize(await _unitOfWork.UserRepository.ToPagination(0, 1000)));
            };
        }

        private async Task SeedingTopicData()
        {
            if (!await _unitOfWork.TopicRepository.AnyAsync())
            {
                string json = File.ReadAllText(LinkConstants.TopicData);
                List<Topic> topics = JsonSerializer.Deserialize<List<Topic>>(json)!;
                await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    await _unitOfWork.TopicRepository.AddRangeAsync(topics);
                });
                Log.Information("Seeding Topic data: " + JsonSerializer.Serialize(await _unitOfWork.TopicRepository.ToPagination(0, 1000)));
            };
        }

        private async Task SeedingBlogData()
        {
            if (!await _unitOfWork.BlogRepository.AnyAsync())
            {
                string json = File.ReadAllText(LinkConstants.BlogData);
                List<Blog> blogs = JsonSerializer.Deserialize<List<Blog>>(json)!;
                await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    await _unitOfWork.BlogRepository.AddRangeAsync(blogs);
                });
                Log.Information("Seeding Blog data: " + JsonSerializer.Serialize(await _unitOfWork.BlogRepository.ToPagination(0, 1000)));
            };
        }
        private async Task SeedingCommentData()
        {
            if (!await _unitOfWork.CommentRepository.AnyAsync())
            {
                string json = File.ReadAllText(LinkConstants.CommentData);
                List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(json)!;
                await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    await _unitOfWork.CommentRepository.AddRangeAsync(comments);
                });
                Log.Information("Seeding Comment data: " + JsonSerializer.Serialize(await _unitOfWork.CommentRepository.ToPagination(0, 1000)));
            };
        }
    }
}