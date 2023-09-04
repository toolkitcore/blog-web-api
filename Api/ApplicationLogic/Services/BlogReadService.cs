using System.Text.Json;
using Api.ApplicationLogic.Interface;
using Api.Core;
using Api.Core.Commons;
using Api.Core.Entities;
using Api.Infrastructure;
using Api.Presentation.Constants;
using Microsoft.EntityFrameworkCore;
using Serilog;


namespace Api.ApplicationLogic.Services
{
    public class BlogReadService : IBlogReadService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;
        private bool _redisOption;
        public BlogReadService(IUnitOfWork unitOfWork,
                               ICacheService cacheService,
                               AppConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _redisOption = configuration.UseRedisCache;
        }
        public async Task<Pagination<Blog>> Get(int pageIndex, int pageSize)
        {
            Log.Information("Request: " + JsonSerializer.Serialize(new { PageIndex = pageIndex, PageSize = pageSize }));
            var blogs = await _unitOfWork.BlogRepository.GetAsync(
                include: x =>
                x.Include(x => x.Topics)
                .Include(x => x.BlogTopics)
                .ThenInclude(x => x.Topic),
                pageIndex: pageIndex,
                pageSize: pageSize);
            Log.Information("Response: " + JsonSerializer.Serialize(blogs));

            return blogs;
        }

        public async Task<Blog> Get(int id)
        {
            Log.Information("Request: " + JsonSerializer.Serialize(id));

            if (!_redisOption)
            {
                var blog = await _unitOfWork.BlogRepository.FirstOrDefaultAsync(
                    filter: x => x.Id == id,
                    include: x => x.Include(x => x.Topics)
                    .Include(x => x.BlogTopics)
                    .ThenInclude(x => x.Topic)
                    .Include(x => x.Comments));
                Log.Information("Response: " + JsonSerializer.Serialize(blog));
                return blog;

            }
            // Attempt to fetch the blog from the cache
            var cachedBlog = await _cacheService.Get<Blog>("blog_" + id);
            if (cachedBlog != null)
            {
                Log.Information("Response: " + JsonSerializer.Serialize(cachedBlog));
                return cachedBlog;
            }
            // Blog not found in cache, fetch from the database
            var result = await _unitOfWork.BlogRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (result == null)
            {
                Log.Error(ErrorConstants.NotFoundMessage);
                throw new ArgumentNullException(ErrorConstants.NotFoundMessage);
            }
            // Add the blog to the cache with a unique key
            var expiryTime = DateTimeOffset.Now.AddMinutes(3);
            await _cacheService.Set("blog_" + id, result, expiryTime);
            Log.Information("Response: " + JsonSerializer.Serialize(result));
            return result;
        }
    }
}