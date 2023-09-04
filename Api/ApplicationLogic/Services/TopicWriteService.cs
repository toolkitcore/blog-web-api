using Api.ApplicationLogic.Interface;
using Api.Core;
using Api.Core.Entities;
using Api.Infrastructure;
using AutoMapper;
using Models.Topic;
using Serilog;
using System.Text.Json;

namespace Api.ApplicationLogic.Services
{
    public class TopicWriteService : ITopicWriteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private bool _redisOption;

        public TopicWriteService(IUnitOfWork unitOfWork,
                                IMapper mapper,
                                ICacheService cacheService,
                                AppConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
            _redisOption = configuration.UseRedisCache;
        }
        public async Task<int> Add(TopicCreateModel request)
        {
            Log.Information("Request: " + JsonSerializer.Serialize(request));

            var topic = _mapper.Map<Topic>(request);
            await _unitOfWork.ExecuteTransactionAsync(async () =>
            {
                await _unitOfWork.TopicRepository.AddAsync(topic);
            });
            if (_redisOption)
                await _cacheService.Remove("topic_" + topic.Id);
            Log.Information("Response: " + JsonSerializer.Serialize(topic.Id));

            return topic.Id;
        }
        public async Task<TopicDTO> Update(TopicUpdateModel request)
        {
            Log.Information("Request: " + JsonSerializer.Serialize(request));
            var topic = await _unitOfWork.TopicRepository.FirstOrDefaultAsync(x => x.Id == request.Id);
            topic = _mapper.Map<Topic>(request);
            await _unitOfWork.ExecuteTransactionAsync(() =>
            {
                _unitOfWork.TopicRepository.Update(topic);
            });
            var result = _mapper.Map<TopicDTO>(topic);
            if (_redisOption)
                await _cacheService.Remove("topic_" + topic.Id);
            Log.Information("Response: " + JsonSerializer.Serialize(topic.Id));

            return result;
        }
        public async Task<int> Delete(int id)
        {
            Log.Information("Request: " + JsonSerializer.Serialize(id));

            var topic = await _unitOfWork.TopicRepository.FirstOrDefaultAsync(x => x.Id == id);
            await _unitOfWork.ExecuteTransactionAsync(() =>
            {
                _unitOfWork.TopicRepository.Delete(topic);
            });
            if (_redisOption)
                await _cacheService.Remove("topic_" + topic.Id);
            Log.Information("Response: " + JsonSerializer.Serialize(topic.Id));

            return topic.Id;
        }
    }
}