using System.Text.Json;
using Api.ApplicationLogic.Interface;
using Api.Core;
using Api.Core.Entities;
using Api.Infrastructure;
using AutoMapper;
using Models.Comment;
using Serilog;

namespace Api.ApplicationLogic.Services
{
    public class CommentWriteService : ICommentWriteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private bool _redisOption;

        public CommentWriteService(IUnitOfWork unitOfWork,
                                IMapper mapper,
                                ICacheService cacheService,
                                AppConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
            _redisOption = configuration.UseRedisCache;
        }
        public async Task<int> Add(CommentCreateModel request)
        {
            Log.Information("Request: " + JsonSerializer.Serialize(request));

            var comment = _mapper.Map<Comment>(request);
            await _unitOfWork.ExecuteTransactionAsync(async () =>
            {
                await _unitOfWork.CommentRepository.AddAsync(comment);
            });
            if (_redisOption)
                await _cacheService.Remove("comment_" + comment.Id);
            Log.Information("Response: " + JsonSerializer.Serialize(comment.Id));

            return comment.Id;
        }
        public async Task<CommentDTO> Update(CommentUpdateModel request)
        {
            Log.Information("Request: " + JsonSerializer.Serialize(request));
            var comment = await _unitOfWork.CommentRepository.FirstOrDefaultAsync(x => x.Id == request.Id);
            comment = _mapper.Map<Comment>(request);
            await _unitOfWork.ExecuteTransactionAsync(() =>
            {
                _unitOfWork.CommentRepository.Update(comment);
            });
            var result = _mapper.Map<CommentDTO>(comment);
            if (_redisOption)
                await _cacheService.Remove("comment_" + comment.Id);
            Log.Information("Response: " + JsonSerializer.Serialize(comment.Id));

            return result;
        }
        public async Task<int> Delete(int id)
        {
            Log.Information("Request: " + JsonSerializer.Serialize(id));

            var comment = await _unitOfWork.CommentRepository.FirstOrDefaultAsync(x => x.Id == id);
            await _unitOfWork.ExecuteTransactionAsync(() =>
            {
                _unitOfWork.CommentRepository.Delete(comment);
            });
            if (_redisOption)
                await _cacheService.Remove("comment_" + comment.Id);
            Log.Information("Response: " + JsonSerializer.Serialize(comment.Id));

            return comment.Id;
        }
    }
}