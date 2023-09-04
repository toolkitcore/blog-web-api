using Api.ApplicationLogic.Interface;
using Api.Core;
using Api.Core.Commons;
using Api.Core.Entities;
using Api.Infrastructure;
using Api.Presentation.Constants;
using Serilog;
using System.Text.Json;


namespace Api.ApplicationLogic.Services
{
    public class CommentReadService : ICommentReadService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;
        private bool _redisOption;

        public CommentReadService(IUnitOfWork unitOfWork,
                               ICacheService cacheService,
                               AppConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _redisOption = configuration.UseRedisCache;
        }


        public async Task<Pagination<Comment>> Get(int pageIndex, int pageSize)
        {
            Log.Information("Request: " + JsonSerializer.Serialize(new { PageIndex = pageIndex, PageSize = pageSize }));
            var comments = await _unitOfWork.CommentRepository.ToPagination(pageIndex, pageSize);

            // Log the comments using System.Text.Json serialization
            Log.Information("Response: " + JsonSerializer.Serialize(comments));

            return comments;
        }

        public async Task<Comment> Get(int id)
        {
            Log.Information("Request: " + JsonSerializer.Serialize(id));

            if (!_redisOption)
            {
                var comment = await _unitOfWork.CommentRepository.FirstOrDefaultAsync(x => x.Id == id);
                Log.Information("Response: " + JsonSerializer.Serialize(comment));
                return comment;

            }
            // Attempt to fetch the comment from the cache
            var cachedComment = await _cacheService.Get<Comment>("comment_" + id);
            if (cachedComment != null)
            {
                Log.Information("Response: " + JsonSerializer.Serialize(cachedComment));
                return cachedComment;
            }
            // Comment not found in cache, fetch from the database
            var result = await _unitOfWork.CommentRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (result == null)
            {
                Log.Error(ErrorConstants.NotFoundMessage);
                throw new ArgumentNullException(ErrorConstants.NotFoundMessage);
            }
            // Add the comment to the cache with a unique key
            var expiryTime = DateTimeOffset.Now.AddMinutes(3);
            await _cacheService.Set("comment_" + id, result, expiryTime);
            Log.Information("Response: " + JsonSerializer.Serialize(result));
            return result;
        }
    }
}