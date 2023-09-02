using Api.ApplicationLogic.Interface;
using Api.Core.Entities;
using Api.Infrastructure;
using Api.Infrastructure.Persistence;
using Api.Presentation.Constants;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json;

namespace Api.ApplicationLogic.Services
{
    public class SeedService : ISeedService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;

        public SeedService(IUnitOfWork unitOfWork, ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
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

            if (!await _unitOfWork.UserRepository.AnyAsync())
            {
                string json = File.ReadAllText(LinkConstants.UserData);
                List<User> users = JsonSerializer.Deserialize<List<User>>(json)!;
                await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    await _unitOfWork.UserRepository.AddRangeAsync(users);
                });
            };

            await _context.Topics.ExecuteDeleteAsync();
            await _context.Blogs.ExecuteDeleteAsync();
            
            await SeedingBlogData();
            await SeedingTopicData();
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
                Log.Information("Seeding Topic data: " + JsonSerializer.Serialize(await _unitOfWork.BlogRepository.ToPagination(0, 1000)));
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
    }
}