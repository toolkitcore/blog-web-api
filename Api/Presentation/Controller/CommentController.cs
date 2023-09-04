using Api.ApplicationLogic.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Comment;

namespace Api.Presentation.Controller
{
    public class CommentController : BaseController
    {
        private readonly ICommentReadService _commentReadService;
        private readonly ICommentWriteService _commentWriteService;
        public CommentController(ICommentReadService commentReadService, ICommentWriteService commentWriteService = null)
        {
            _commentReadService = commentReadService;
            _commentWriteService = commentWriteService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
            => Ok(await _commentReadService.Get(id));

        [HttpGet]
        public async Task<IActionResult> Get(int pageIndex = 0, int pageSize = 10)
            => Ok(await _commentReadService.Get(pageIndex, pageSize));
        [HttpPost]
        public async Task<IActionResult> Add(CommentCreateModel request)
            => Ok(await _commentWriteService.Add(request));

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update(CommentUpdateModel request)
            => Ok(await _commentWriteService.Update(request));

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
            => Ok(await _commentWriteService.Delete(id));
    }
}