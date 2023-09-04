using Api.ApplicationLogic.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Blog;

namespace Api.Presentation.Controller
{
    public class BlogController : BaseController
    {
        private readonly IBlogReadService _blogReadService;
        private readonly IBlogWriteService _blogWriteService;
        public BlogController(IBlogReadService blogReadService, IBlogWriteService blogWriteService = null)
        {
            _blogReadService = blogReadService;
            _blogWriteService = blogWriteService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
            => Ok(await _blogReadService.Get(id));

        [HttpGet]
        public async Task<IActionResult> Get(int pageIndex = 0, int pageSize = 10)
            => Ok(await _blogReadService.Get(pageIndex, pageSize));
        [HttpPost]
        public async Task<IActionResult> Add(BlogCreateModel request)
                   => Ok(await _blogWriteService.Add(request));

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update(BlogUpdateModel request)
            => Ok(await _blogWriteService.Update(request));

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
            => Ok(await _blogWriteService.Delete(id));

    }
}