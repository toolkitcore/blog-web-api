using Api.ApplicationLogic.Interface;
using Api.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Book;
using NewRelic.Api.Agent;

namespace Api.Presentation.Controller
{

    public class BookController : BaseController
    {
        private readonly IBookReadService _bookReadService;
        private readonly IBookWriteService _bookWriteService;
        public BookController(
            IBookReadService bookReadService,
            IBookWriteService bookWriteService)
        {
            _bookReadService = bookReadService;
            _bookWriteService = bookWriteService;
        }
        [HttpGet("{id}")]
        [Transaction]
        public async Task<IActionResult> Get(int id)
            => Ok(await _bookReadService.Get(id));

        [HttpGet]
        public async Task<IActionResult> Get(int pageIndex = 0, int pageSize = 10)
            => Ok(await _bookReadService.Get(pageIndex, pageSize));

        [HttpPost]
        [Transaction]
        public async Task<IActionResult> Add(BookDTO request)
            => Ok(await _bookWriteService.Add(request));

        [Authorize]
        [HttpPut]
        [Transaction]
        public async Task<IActionResult> Update(Book request)
            => Ok(await _bookWriteService.Update(request));

        [Authorize]
        [HttpDelete]
        [Transaction]
        public async Task<IActionResult> Delete(int id)
            => Ok(await _bookWriteService.Delete(id));
    }
}