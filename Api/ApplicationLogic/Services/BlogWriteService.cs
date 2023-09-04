using System.Text.Json;
using Api.ApplicationLogic.Interface;
using Api.Core;
using Api.Core.Entities;
using Api.Infrastructure;
using AutoMapper;
using Models.Blog;
using Serilog;


namespace Api.ApplicationLogic.Services
{
    public class BlogWriteService : IBlogWriteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private bool _redisOption;

        public BlogWriteService(IUnitOfWork unitOfWork,
                                IMapper mapper,
                                ICacheService cacheService,
                                AppConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
            _redisOption = configuration.UseRedisCache;
        }
        public async Task<int> Add(BlogCreateModel request)
        {
            Log.Information("Request: " + JsonSerializer.Serialize(request));

            var blog = _mapper.Map<Blog>(request);
            await _unitOfWork.ExecuteTransactionAsync(async () =>
            {
                await _unitOfWork.BlogRepository.AddAsync(blog);
            });
            if (_redisOption)
                await _cacheService.Remove("blog_" + blog.Id);
            Log.Information("Response: " + JsonSerializer.Serialize(blog.Id));

            return blog.Id;
        }
        public async Task<BlogCreateModel> Update(BlogUpdateModel request)
        {
            Log.Information("Request: " + JsonSerializer.Serialize(request));
            var blog = await _unitOfWork.BlogRepository.FirstOrDefaultAsync(x => x.Id == request.Id);
            blog = _mapper.Map<Blog>(request);
            await _unitOfWork.ExecuteTransactionAsync(() =>
            {
                _unitOfWork.BlogRepository.Update(blog);
            });
            var result = _mapper.Map<BlogCreateModel>(blog);
            if (_redisOption)
                await _cacheService.Remove("blog_" + blog.Id);
            Log.Information("Response: " + JsonSerializer.Serialize(blog.Id));

            return result;
        }
        public async Task<int> Delete(int id)
        {
            Log.Information("Request: " + JsonSerializer.Serialize(id));

            var blog = await _unitOfWork.BlogRepository.FirstOrDefaultAsync(x => x.Id == id);
            await _unitOfWork.ExecuteTransactionAsync(() =>
            {
                _unitOfWork.BlogRepository.Delete(blog);
            });
            if (_redisOption)
                await _cacheService.Remove("blog_" + blog.Id);
            Log.Information("Response: " + JsonSerializer.Serialize(blog.Id));

            return blog.Id;
        }
    }
}