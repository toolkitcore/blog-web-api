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
    public class TopicReadService : ITopicReadService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;
        private bool _redisOption;

        public TopicReadService(IUnitOfWork unitOfWork,
                               ICacheService cacheService,
                               AppConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _redisOption = configuration.UseRedisCache;
        }


        public async Task<Pagination<Topic>> Get(int pageIndex, int pageSize)
        {
            Log.Information("Request: " + JsonSerializer.Serialize(new { PageIndex = pageIndex, PageSize = pageSize }));
            var topics = await _unitOfWork.TopicRepository.ToPagination(pageIndex, pageSize);

            // Log the topics using System.Text.Json serialization
            Log.Information("Response: " + JsonSerializer.Serialize(topics));

            return topics;
        }

        public async Task<Topic> Get(int id)
        {
            Log.Information("Request: " + JsonSerializer.Serialize(id));

            if (!_redisOption)
            {
                var topic = await _unitOfWork.TopicRepository.FirstOrDefaultAsync(x => x.Id == id);
                Log.Information("Response: " + JsonSerializer.Serialize(topic));
                return topic;

            }
            // Attempt to fetch the topic from the cache
            var cachedTopic = await _cacheService.Get<Topic>("topic_" + id);
            if (cachedTopic != null)
            {
                Log.Information("Response: " + JsonSerializer.Serialize(cachedTopic));
                return cachedTopic;
            }
            // Topic not found in cache, fetch from the database
            var result = await _unitOfWork.TopicRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (result == null)
            {
                Log.Error(ErrorConstants.NotFoundMessage);
                throw new ArgumentNullException(ErrorConstants.NotFoundMessage);
            }
            // Add the topic to the cache with a unique key
            var expiryTime = DateTimeOffset.Now.AddMinutes(3);
            await _cacheService.Set("topic_" + id, result, expiryTime);
            Log.Information("Response: " + JsonSerializer.Serialize(result));
            return result;
        }
    }
}