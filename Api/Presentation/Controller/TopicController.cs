using Api.ApplicationLogic.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Comment;
using Models.Topic;

namespace Api.Presentation.Controller
{
    public class TopicController : BaseController
    {
        private readonly ITopicReadService _topicReadService;
        private readonly ITopicWriteService _topicWriteService;
        public TopicController(ITopicReadService topicReadService, ITopicWriteService topicWriteService = null)
        {
            _topicReadService = topicReadService;
            _topicWriteService = topicWriteService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
            => Ok(await _topicReadService.Get(id));

        [HttpGet]
        public async Task<IActionResult> Get(int pageIndex = 0, int pageSize = 10)
            => Ok(await _topicReadService.Get(pageIndex, pageSize));
        [HttpPost]
        public async Task<IActionResult> Add(TopicCreateModel request)
                   => Ok(await _topicWriteService.Add(request));

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update(TopicUpdateModel request)
            => Ok(await _topicWriteService.Update(request));

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
            => Ok(await _topicWriteService.Delete(id));
    }
}